# Build Instructions for MatterSphere

To build MatterSphere from the source code, please follow these steps:

1. **Ensure all required software and components are installed.** See the sections below for more details.
2. **Clone the repository** and checkout the `develop` branch.
3. **Prepare the source code.** See the Source Code Preparation section below.
4. **Open the solution in Visual Studio** and build it.

## Prerequisites

### 1. Install Visual Studio 2022
Make sure Visual Studio 2022 is installed with the necessary workloads, components, and extensions.

#### Required Workloads and Components
**Workloads:**
- .NET desktop development
- Desktop development with C++
- Data storage and processing
- ASP.NET and web development
- Office/SharePoint development

**Components:**
- C++ MFC for latest v143 build tools (x86 & x64)
- Windows Workflow Foundation

#### Required Visual Studio 2022 Extensions
- HeatWave for VS2022

### 2. Install Software with Commercial Licenses
Make sure the following commercial software is purchased and installed:

| Software | Required for building |
|-|-|
|Actipro SyntaxEditor v4.0.0290 for Windows Forms + SyntaxEditor .NET Languages Add-on| Core, Workflow |
| Aspose.Total for .NET 22.9 | Core, PDFBundler, ESIndex, MCEP |
| Telerik UI for WinForms 2018.2.621 | Core, HighQ Add-in, Workflow Add-in |
| Infragistics UI for WinForms 19.2 | Core, Security |
| Outlook Redemption 6.3 | Core, Office Add-in, MCEP, OMSExport
| Dynamic .NET TWAIN 8.3.3 | Document Scanner Add-in |
| Crystal Reports 13.0.25 | Core |
| iManage Work SDK 10.8.0.98 | iManage Work 10 Plugin |

## Source Code Preparation

1. **Replace the Aspose license:**
   - Replace the file `.\References\Aspose\Aspose.Total.lic` with the license you purchased.

2. **Update the Document Scanner license key:**
   - Open the file `.\Addins\Document Scanner\FWBS.Scanning\ucScanning.cs` and replace the value of the `dsProductKey` constant with your license key.

3. **Download and set up Outlook Redemption:**
   - Download Outlook Redemption and unzip it to the `.\References\Redemption` folder.

4. **Download and set up the iManage Work SDK:**
   - Download the iManage Work SDK and unzip it to the `.\References\iManageWork10` folder.

## Building the Solution

1. Open Visual Studio 2022.
2. Open the required solution file.
3. Build the solution by selecting **Build > Build Solution** from the menu or by pressing `Ctrl+Shift+B`.

## Solution List

### Core and Office Add-ins

| Solution | Path |
|-|-|
| Core | .\Core\Core.sln |
| Shell | ".\Interops\Shell\Shell.sln" |
| Document Previewer | ".\Core\Document Previewer\DocumentPreviewer.sln" |
| Workflow Add-in | ".\Addins\Workflow4\Workflow.sln" |
| Workflow | ".\Workflow\Workflow.sln" |
| Document Scanner | ".\Addins\Document Scanner\FWBS.Scanning.sln" |
| FPEIntegration | ".\Addins\eForms\FPEIntegration.sln" |
| File Management | ".\Addins\File Management\FileManagement.sln" |
| DashboardFinancialTile | ".\Addins\DashboardFinancialTile\DashboardFinancialTile.sln" |
| HighQ Add-in | ".\Addins\HighQ\HighQ.sln" |
| QuickClientCreation | ".\Addins\OMSQuickClient\QuickClientCreation\QuickClientCreation.sln" |
| LegalForms | ".\Addins\LegalForms\LegalForms.sln" |
| Security | ".\Addins\Security\Fwbs.Oms.Addin.Security.sln" |
| SP2010 | ".\Addins\SP2010\SP2010.sln" |
| MatterSphereIntegration | ".\Addins\MatterSphere\MatterSphereIntegration.sln" |
| iManage Work 10 Plugin | ".\Addins\iManage Work 10 Plugin\iManageWork10MSPlugin.sln" |
| Office ControlShim | ".\Office\Office.sln" |
| Office Add-in | ".\Office2016\Office2016.sln" |
| PackageUpgradeAnalyzer | ".\Setup\PackageUpgradeAnalyzer.sln" |

### Installers

| Name | Path |
|-|-|
| Core Installer | ".\Setup\SetupProject.sln" |

### Database Projects (DACPACs)

| Name | Path |
|-|-|
| MatterSphere Standard | ".\Database\MatterSphere_Standard\MatterSphere.sln" |
| MatterSphere Upgrade | ".\Database\MatterSphere_Upgrade\MatterSphere_Upgrade\MatterSphere_Upgrade.sln" |

### Services

| Name | Path |
|-|-|
| DocumentArchivingService | ".\Services\DocumentArchivingService\DocumentArchivingService.sln" |
| MCEP | ".\Services\MCEP - EWS\MCEP EWS.sln" |
| EWSSync | ".\Services\MatterSphereEWSSync\MatterSphereEWSSync.sln" |
| ESIndex | ".\Services\ESIndex\ESIndex.sln" |
| MSIndex | ".\Services\MSIndex\MSIndex.sln" |
| PDFBundler | ".\Services\Bundler Service\MatterSphere Bundler Service.sln" |
| OMSExport | ".\Services\OMSExport\OMSExport.sln" |
| ADSync | ".\Services\ActiveDirectoryUsers\ActiveDirectoryUsers.sln" |
