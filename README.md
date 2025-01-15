# Fall Guys CMS Tool
<div align="center">
    <img src="https://github.com/floyzi/FallGuys-CMSTool/blob/master/Assets/GithubImages/Screenshot1.png" alt="How it looks">
</div>
<p align="center">An easy to use tool to decrypt and encrypt Fall Guys content files.<br>Support both, content_v1 & content_v2 versions</p>

## Features
- Content decryption in different ways 
- Custom XorKey 
- Encryption in both ways. Content_v1 & content_v2

## Usage
- Extract [latest release](https://github.com/floyzi/FallGuys-CMSTool/releases/latest) into the blank folder and launch the .exe
### Decryption
- Select `content_v2.gdata` or `content_v1` file (can be found in `AppData/LocalLow/Mediatonic/FallGuys_client/`)
- Hit the `"Decrypt"` button and wait for a moment
- Once done check the `"Decrypted_Output"` folder (`Open -> Decoded Output`) for the decrypted content file

### Encryption
- Select `.json` file of your content (or `_meta.json` if you decrypted content into parts)
- Hit the `"Encrypt"` button and wait for a moment 
- Once done check the `"Encrypted_Output"` folder (`Open -> Encoded Output`). There you'll find the `content_v2.gdata` file (or `content_v1`, if you selected it in settings)

## Credits 
- Made using [Avalonia UI](https://github.com/AvaloniaUI/Avalonia)
- Some UI code were taken from [this repository](https://github.com/M0n7y5/FenixProFmod)