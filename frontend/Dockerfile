FROM node:latest AS build

WORKDIR /build

COPY . .

RUN npm run build

FROM httpd:alpine AS app

WORKDIR /usr/local/apache2/htdocs/

COPY --from=build /build/build/ .
