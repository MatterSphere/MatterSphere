# Excel, Outlook, Word VSTO Add-in Projects

This is a brief guide on how to configure and work with PFX in the VSTO add-in projects.

## Requirements

To work with these projects, you need the following:

- Visual Studio 2022
- .NET Framework 4.8
- VSTO (Visual Studio Tools for Office) components
- A .pfx file containing a code signing certificate to sign the VSTO add-in. We recommend each developer create their own self-signed certificate for development purposes.

## Create a Self-Signed Certificate via Visual Studio

You can create a self-signed certificate directly within Visual Studio using these steps:

1. Right-click on your project in the Solution Explorer and select **Properties**.
2. Navigate to the **Signing** tab.
3. Check the **Sign the ClickOnce manifests** checkbox.
4. Click on **Create Test Certificate**.
5. You can optionally provide a password for your .pfx file or leave it blank for no password.
6. Click **OK**.

This will create a new .pfx file, and it will be used to sign the ClickOnce manifests for your project. The .pfx file is placed in your project directory.

**Note** This certificate is only for testing purposes. For a production VSTO Add-In, you should obtain a certificate from a trusted Certificate Authority (CA).

## Caution

Please keep your .pfx files and passwords secure and do not check them into public version control. These files are sensitive as they contain your private keys. Anyone who has access to these files and passwords can sign code in your name.