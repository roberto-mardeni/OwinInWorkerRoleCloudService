$cert = New-SelfSignedCertificate -DnsName demo.doralmatrix.zzz -CertStoreLocation "cert:\LocalMachine\My" -KeyLength 2048 -KeySpec "KeyExchange"
$password = ConvertTo-SecureString -String "your-password" -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath ".\demo.doralmatrix.zzz.pfx" -Password $password