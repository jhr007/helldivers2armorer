FROM nginx:alpine

COPY . /usr/share/nginx/html

# SPA fallback: serve index.html for all routes Nginx doesn't find as files
RUN printf 'server {\n\
    listen 80;\n\
    root /usr/share/nginx/html;\n\
    location / {\n\
        try_files $uri $uri/ /index.html;\n\
    }\n\
}\n' > /etc/nginx/conf.d/default.conf

EXPOSE 80
