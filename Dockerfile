# STAGE - build base image with libvips compilation
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

RUN apt-get update 
RUN apt-get install -y \
	build-essential \
	pkg-config \
	unzip \
	wget

# libvips dependencies
RUN apt-get install -y \
	glib-2.0-dev \
	liblcms2-dev \
	libexpat-dev \
	librsvg2-dev \
	libpng-dev \
	libgif-dev \
	libtiff5-dev \
	libjpeg-dev \
	libgsf-1-dev \
	libexif-dev \
	liblcms2-dev \
	libheif-dev \
	liborc-dev \
	libx265-dev \
	libde265-dev \
	libaom-dev

ARG VIPS_VERSION=8.10.6
ARG VIPS_URL=https://github.com/libvips/libvips/releases/download

WORKDIR /usr/local/src

RUN wget ${VIPS_URL}/v${VIPS_VERSION}/vips-${VIPS_VERSION}.tar.gz \
	&& tar xzf vips-${VIPS_VERSION}.tar.gz \
	&& cd vips-${VIPS_VERSION} \
	&& ./configure \
	&& make V=0 \
	&& make install \
	&& ldconfig

# STAGE - compile app
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source
# copy csproj and restore as distinct layers
COPY *.sln .
COPY scaler/*.csproj ./scaler/
RUN dotnet restore

# copy everything else and build app
COPY scaler/. ./scaler/
WORKDIR /source/scaler
RUN dotnet publish -c Release -o /app --no-cache

# STAGE - create image
FROM base AS image
WORKDIR /app
COPY --from=build /app ./
RUN mkdir output
ENTRYPOINT ["dotnet", "scaler.dll"]
