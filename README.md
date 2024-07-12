# LSports Trade360 SDK

## Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
    - [Pre-requisites](#pre_requisites)
    - [Supported .NET Versions](#supported_versions)
    - [Installing](#installing)
    - [Initial Configuration](#configuration)
- [Usage Guide](#usage_guide)
  - [Connecting to Trade360 Feed](#usage_guide_feed)
  - [Using the Snapshot API](#usage_snapshot_api)
  - [Using Customers API](#usage_customers_api)
- [Contribution](#contributing)
- [License](#license)

## About <a name = "about"></a>

The Trade360 SDK aims to simplify the integration with Trade360 services. 
This SDK provides a comprehensive set of tools and examples to streamline connecting to the Trade360 feed, utilizing the snapshot API, and interacting with the customers API. 

### Key Features
- Efficiently connect and interact with Trade360 feed.
- Utilize the snapshot API for real-time recovery.
- Manage customer data and subscriptions seamlessly via the Customers Api.

## Getting Started <a name = "getting_started"></a>

This section provides examples and guidance to help you start using the Trade360 SDK.

- Feed Examples
- Customers API Examples
- Snapshot Examples

### Prerequisites <a name = "pre_requisites"></a>

Ensure you have the following installed on your machine:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Supported .NET Versions <a name = "supported_versions"></a>

This SDK targets .NET Standard 2.1 and is compatible with the following .NET implementations:

| .NET Implementation | Supported Versions         |
| ------------------- | -------------------------- |
| .NET Core           | :heavy_check_mark: 3.0, 3.1, 5.0, 6.0, 7.0  |
| .NET                | :heavy_check_mark: 5.0, 6.0, 7.0  |
| Mono                | :heavy_check_mark: 6.4 and later    |
| Xamarin.iOS         | :heavy_check_mark: 10.14 and later  |
| Xamarin.Mac         | :heavy_check_mark: 3.8 and later    |
| Xamarin.Android     | :heavy_check_mark: 8.0 and later    |
| Unity               | :heavy_check_mark: 2018.1 and later |

## Installing <a name = "installing"></a>

A step-by-step series of instructions to set up your development environment.

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/trade360sdk-feed-example.git
    cd trade360sdk-feed-example
    ```

2. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

3. **Build the project:**

    ```bash
    dotnet build
    ```

## Initial Configuration <a name = "configuration"></a>

Provide initial configuration examples for:

- Feed configuration
- Snapshot API
- Customers API

## Usage Guide <a name = "usage_guide"></a>

### Connecting to Trade360 Feed <a name = "usage_guide_feed"></a>

Include detailed instructions and examples for connecting to the Trade360 feed.

### Using the Snapshot API <a name = "usage_snapshot_api"></a>

Provide guidance on how to use the snapshot API with code examples.

### Using Customers API <a name = "usage_customers_api"></a>

Describe the process of using the customers API, including example requests and responses.

## Contributing <a name = "contributing"></a>

Please read [CONTRIBUTING.md](../CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## License <a name = "license"></a>

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Replace `[github-releases]` with the actual link to your GitHub releases page.
