# pWallet 2.0
This is a UI for Phoenixd Server by ACINQ.

Its built in .NET 9 Blazor as a web application with PWA support so its "installable" through browser on your phone by adding it to home screen.

You can also if you host or handle your DNS to your domain add cloudflare api credentials to add custom BIP353 address to your phoenixd server.

You can try it at https://pwallet.app but it do requires an account and since i havent made any encryption to either backend data or cloudflare data i wouldnt suggest you to add any credentials to your backend in the test-application since its fully readable in the database.
With that said, this is supposed to be a self-hosted UI alongside self hosting phoenixd server.

As for now there is only one built version in the repo and that is for Windows users.
But since its .NET you can build the project and run it on any OS.

It works with a SQLite database for account management and also to save your backend and cloudflare credentials.

The UI with Phoenix Server can handle:
- Lightning invoices (pay and receive)
- LNURL (pay)
- Lightning Address (pay)
- Bolt12 offer (pay and receive)
- Bip353 address (pay and receive)
- LNAuth
- Onchain (pay)

If you have a domain and handles dns for it through Cloudflare you can also:
- Create custom Bip353 address with your domain
