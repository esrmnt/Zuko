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
        //Console.WriteLine(args.Length);

        await Get_ApiManagementGetApiContract();
    }


    public static async Task Get_ApiManagementGetApiContract()
        {

            ResourceIdentifier prodId = null;
            // Generated from example definition: specification/apimanagement/resource-manager/Microsoft.ApiManagement/stable/2021-08-01/examples/ApiManagementGetApiContract.json
            // this example is just showing the usage of "Api_Get" operation, for the dependent resources, they will have to be created separately.

            // get your azure access token, for more details of how Azure SDK get your access token, please refer to https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication?tabs=command-line
            TokenCredential cred = new DefaultAzureCredential();
            // authenticate your client
            ArmClient client = new ArmClient(cred);

            // this example assumes you already have this ApiManagementServiceResource created on azure
            // for more information of creating ApiManagementServiceResource, please refer to the document of ApiManagementServiceResource
            string subscriptionId = "b4a9bb8e-85e1-4f6f-a776-06a30b6944ea";
            string resourceGroupName = "esrmnt-ws-rg";
            string serviceName = "esrmnt-apim-dev";
            ResourceIdentifier apiManagementServiceResourceId = ApiManagementServiceResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, serviceName);
            ApiManagementServiceResource apiManagementService = client.GetApiManagementServiceResource(apiManagementServiceResourceId);

            // get the collection of this ApiResource
            var collection = apiManagementService.GetApis();


            //System.Console.WriteLine( apiResource.Id);

            // var operations = apiResource.GetApiOperations();

            // foreach (var operation in operations)
            // {
            //     System.Console.WriteLine(operation.Id);
            // }
            // invoke the operation and iterate over the result
            await foreach (ApiResource item in collection.GetAllAsync())
            {
                // the variable item is a resource, you could call other operations on this instance as well
                // but just for demo, we get its data from this resource instance
                ApiData resourceData = item.Data;

                System.Console.WriteLine(resourceData.Name);

                // for demo we just print out the id
                Console.WriteLine($"Succeeded on id: {resourceData.Id}");

                var operations = item.GetApiOperations();
                foreach (var operation in operations)
                {
                    System.Console.WriteLine(operation.Id);

                    var operationPolicies = operation.GetApiOperationPolicies();
                    
                    foreach (var policy in operationPolicies)
                    {
                        System.Console.WriteLine(policy.Data.Value);
                    }
                }

                var products = item.GetApiProducts();

                foreach (var product in products)
                {
                    prodId = product.Id;
                    System.Console.WriteLine( product.Id);
                    System.Console.WriteLine(   product.HasData);
                    //var productPolicies = product.GetApiManagementProductPolicies();
                    var productPolicies = product.Data;

                    System.Console.WriteLine(   productPolicies.Name + "---" );



                    
                }


            }

//  var apimClient = new ApiManagementClient(subscriptionId, credential);
            // Get product policy
        var productPolicy = apiManagementService.GetApiManagementProduct(prodId.Name);

        // Display policy
        Console.WriteLine($"Policy applied to product {prodId} associated with API :");
        Console.WriteLine(productPolicy.Value.Data);

        foreach (var item in productPolicy.Value.GetApiManagementProductPolicies())
        {
            System.Console.WriteLine(item.Data.Value);
        } 

            Console.WriteLine($"Succeeded");

        }
}

