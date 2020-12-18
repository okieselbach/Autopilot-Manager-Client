# Autopilot-Manager-Client

A small client to gather Autopilot information and send them to the [Autopilot-Manager](https://github.com/okieselbach/Autopilot-Manager).

## Prerequisites

Requires the .NET Framework 4.6. The .NET version 4.6 is used explicitly, as it defines the lowset common .NET version for Windows 10. Even the first Windows 10 versions are coming with .NET Framework 4.6. So we do not introduce any dependency here.

## Usage

Simply starting the program during OOBE [Shift] + [F10] and providing the Autopilot-Manager app service address as parameter.

## License

Autopilot-Manager-Client is available as open source under the [GPL](LICENSE).

Autopilot-Manager-Client contains code from:
* Raffael Herrmann (see https://github.com/codebude/QRCoder), which is available under the [MIT](https://licenses.nuget.org/MIT) license.
* James Newton-King (see https://www.newtonsoft.com/json), which is available under the [MIT](https://licenses.nuget.org/MIT) license.
* Simon Cropp (see https://github.com/Fody/Costura), which is available under the [MIT](https://licenses.nuget.org/MIT) license.

## Contributing

You may write documentation and source code, pull requests are welcome! You need to provide your contributions under some GPL-compatible license.
