using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.ApiManagement;
using Azure.ResourceManager.ApiManagement.Models;

class Program
{
    static async Task Main(string[] args)
    {
        // Display the number of command line arguments.
        Console.WriteLine(args.Length);

        await Get_ApiManagementGetApiContract();
    }


    public static async Task Get_ApiManagementGetApiContract()
        {
            // Generated from example definition: specification/apimanagement/resource-manager/Microsoft.ApiManagement/stable/2021-08-01/examples/ApiManagementGetApiContract.json
            // this example is just showing the usage of "Api_Get" operation, for the dependent resources, they will have to be created separately.

            // get your azure access token, for more details of how Azure SDK get your access token, please refer to https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication?tabs=command-line
            TokenCredential cred = new DefaultAzureCredential();
            // authenticate your client
            ArmClient client = new ArmClient(cred);

            // this example assumes you already have this ApiManagementServiceResource created on azure
            // for more information of creating ApiManagementServiceResource, please refer to the document of ApiManagementServiceResource
            string subscriptionId = "b4a9bb8e-85e1-4f6f-a776-06a30b6944ea";
            string resourceGroupName = "esrmnt-rg-si";
            string serviceName = "esrmnt-cons-apim-si";
            ResourceIdentifier apiManagementServiceResourceId = ApiManagementServiceResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, serviceName);
            ApiManagementServiceResource apiManagementService = client.GetApiManagementServiceResource(apiManagementServiceResourceId);

            // get the collection of this ApiResource
            ApiCollection collection = apiManagementService.GetApis();

            // invoke the operation and iterate over the result
            await foreach (ApiResource item in collection.GetAllAsync())
            {
                // the variable item is a resource, you could call other operations on this instance as well
                // but just for demo, we get its data from this resource instance
                ApiData resourceData = item.Data;

                var apiOperations = item.GetApiOperations();

                foreach (var op in apiOperations)
                {
                    System.Console.WriteLine((op.GetAsync()).Result);
                }

                System.Console.WriteLine(resourceData.Name);
                System.Console.WriteLine(resourceData.Path);

                // for demo we just print out the id
                Console.WriteLine($"Succeeded on id: {resourceData.Id}");
            }

            Console.WriteLine($"Succeeded");

        }
}

