# CI
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS ci 
ARG BuildConfiguration=Release
WORKDIR /src
COPY . .
RUN dotnet publish "Order.DDD.Demo.sln" -c $BuildConfiguration -o /app/publish -p:UseAppHost=false --os linux --arch x64

# CD
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS cd
EXPOSE 8080
RUN sed -i 's/openssl_conf = openssl_init/#openssl_conf = openssl_init/' /etc/ssl/openssl.cnf
RUN sed -i '1i openssl_conf = default_conf' /etc/ssl/openssl.cnf && \
    echo "\n[ default_conf ]\nssl_conf = ssl_sect\n[ssl_sect]\nsystem_default = system_default_sect\n[system_default_sect]\nMinProtocol = TLSv1\nCipherString = DEFAULT:@SECLEVEL=0" >> /etc/ssl/openssl.cnf
WORKDIR /app
COPY --from=ci /app/publish .