# pWallet 2.0
This is a UI for Phoenixd Server by ACINQ.

Its built in .NET 9 Blazor as a web application with PWA support so its "installable" through browser on your phone by adding it to home screen but can be used on a laptop through browser ( even here installable because of PWA support ).

You can also if you host or handle your DNS to your domain add cloudflare api credentials to add custom BIP353 address to your phoenixd server.

As for now there is a Linux build release, a windows build release, and there is also a Dockerfile.
But since its .NET you can build the project and run it on any OS.

It works with a SQLite database for account management and also to save your cloudflare credentials.
Your Phoenixd Server credentials is added in appsettings.json

***As for now the application restricts user registrations to only ONE user***

The UI with Phoenix Server can handle:
- Lightning invoices (pay and receive)
- LNURL (pay and receive, LNURL string makes static QR to receive sats whenever)
- LNURL-P (receive at zap@domain.com)
- Lightning Address (pay and receive, see LNURL-P)
- Bolt12 offer (pay and receive)
- Bip353 address (pay and receive)
- LNAuth
- Onchain (pay)

Extra:
- It shows Sats, Btc, USD, EUR by clicking your current amount on home screen

If you have a domain and handles dns for it through Cloudflare you can also:
- Create custom Bip353 address with your domain
- Use the domain for LNURL string and LNURL-P addresses (zap@domain.com is activated and working).

**BEFORE YOU START USING THE APPLICATION**
1. Chose how to run! .exe for windows in Windows build, .dll for Linux ( needs ASP.NET Core Runtime for .NET 9 on your system ), and Dockerfile!
2. Fill the appsettings.json with your credentials, if you run through Docker, make sure you edit appsettings.json before building image.
3. Go to wwwroot/.well-known/lnurlp and copy the example file, open the copy and edit the domain, then name the file whatever you wanna use as a lightningaddress.
   If you want another lnurl-p address, just copy the example file again and name it and fix the domain in it.
   Then restart the application!
   If you run through Docker, you need to add files in Dockercontainer ( /wwwroot/.well-known/lnurlp )


<img src="https://github.com/user-attachments/assets/be4a156f-36ad-4299-86ba-3ecfba68481c" width="200px"/>
<img src="https://github.com/user-attachments/assets/54a9148b-731e-44fa-98ad-29f44d551d9e" width="200px"/>
<img src="https://github.com/user-attachments/assets/57e74927-266a-4561-9c25-f02d054500ba" width="200px"/>
<img src="https://github.com/user-attachments/assets/155c84c8-4c8a-418d-af03-3e51b9487159" width="200px"/>
<img src="https://github.com/user-attachments/assets/01ed6e16-4b03-4e13-9308-07aac58ff0b7" width="200px"/>
<img src="https://github.com/user-attachments/assets/4c682ea1-fcd4-45ba-871c-d9e17672181d" width="200px"/>
<img src="https://github.com/user-attachments/assets/223f2f2d-2fb1-4974-8ab8-c3a0858f8725" width="200px"/>
<img src="https://github.com/user-attachments/assets/eda23b42-5514-43e9-95a0-ae8923c270b8" width="200px"/>
<img src="https://github.com/user-attachments/assets/891c64c3-0e9b-47ea-bfff-f2a732440dd1" width="200px"/>
<img src="https://github.com/user-attachments/assets/08e75a7b-48c3-47ed-bf1c-0b53a74b3e02" width="200px"/>
<img src="https://github.com/user-attachments/assets/40530ed3-8dac-459d-90d3-0b668ad274c4" width="200px"/>


