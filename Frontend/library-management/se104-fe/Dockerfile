# syntax=docker/dockerfile:1

ARG NODE_VERSION=20.18.1
ARG VITE_BACKEND_URL=/

FROM node:${NODE_VERSION}-alpine AS build
WORKDIR /app

ENV VITE_BACKEND_URL=${VITE_BACKEND_URL}

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:1.27-alpine
WORKDIR /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist .

EXPOSE 80
