FROM node:24 AS build
WORKDIR /app

ARG VITE_BASE_API_URL
ENV VITE_BASE_API_URL=${VITE_BASE_API_URL}

COPY package*.json ./
RUN npm ci

COPY . .

RUN npm run build

FROM nginx:stable-alpine AS runtime

COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
