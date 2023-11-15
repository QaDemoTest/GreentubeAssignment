using NUnit.Framework;
using PetStoreApiTest.ExtentReporting;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PetStoreApiTest
{
    public class PetApiTest
    {
        string BaseUrl = TestContext.Parameters.Get("BaseUrl");
        string CurrentDirextory = Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory);
        string PetNameAdded = "";

        [Test]
        public void PetsApiTest()
        {
            AddNewPetToStore();
            FindPetById();
            UpdateAnExistingPet();
            DeletePet();
            FindPetWithInvalidPetId();
        }

        /// <summary>
        /// Post request to add new pet to the store
        /// Store the name of the pet added
        /// Assert the status code and the name of the pet added
        /// </summary>
        public void AddNewPetToStore()
        {
            ExtentTestManager.CreateTest("Add new pet to store test");
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("v2/pet");
            request.AddBody(File.ReadAllText(string.Concat(CurrentDirextory, "/../../Api_RequestBody/PetsApi_RequestData/AddNewPet.json")));
            var response = client.ExecutePost(request);
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", BaseUrl + "v2/pet"));
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", (int)response.StatusCode));
            var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;
            PetNameAdded = $"{data["name"]}";
            Console.WriteLine(string.Concat("Name of new pet added :", PetNameAdded));

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(PetNameAdded, Is.EqualTo("Max"));
        }

        /// <summary>
        /// Get request to find the pet added using the pet ID
        /// Assertion to verify the pet added is found
        /// </summary>
        public void FindPetById()
        {
            ExtentTestManager.CreateTest("Find pet by ID test");
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("v2/pet/99");
            var response = client.ExecuteGet(request);
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", BaseUrl + "v2/pet/99"));
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", (int)response.StatusCode));
            var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;
            Console.WriteLine(string.Concat("Name of the pet found: ", $"{data["name"]}"));

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That($"{data["name"]}", Is.EqualTo(PetNameAdded));
        }

        /// <summary>
        /// Get request to find pet with invalid pet ID
        /// Assert for 404 status code
        /// </summary>
        public void FindPetWithInvalidPetId()
        {
            ExtentTestManager.CreateTest("Find pet with invalid pet ID test");
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("v2/pet/-1");
            var response = client.ExecuteGet(request);
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", BaseUrl + "v2/pet/-1"));
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", (int)response.StatusCode));
            var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;

            Assert.That((int)response.StatusCode, Is.EqualTo(404));
            Assert.That($"{data["message"]}", Is.EqualTo("Pet not found"));
        }

        /// <summary>
        /// Put request to update the pet added
        /// Assert the status code and name to ensure the pet is updated
        /// </summary>
        public void UpdateAnExistingPet()
        {
            ExtentTestManager.CreateTest("Update an existing test");
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("v2/pet");
            request.AddBody(File.ReadAllText(string.Concat(CurrentDirextory, "/../../Api_RequestBody/PetsApi_RequestData/UpdateExistingPet.json")));
            var response = client.ExecutePut(request);
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", BaseUrl + "v2/pet"));
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", (int)response.StatusCode));
            var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That($"{data["name"]}", Is.EqualTo("Leo"));
        }

        /// <summary>
        /// Delete request to delete the pet
        /// Assert the status code and the pet ID of the pet deleted
        /// </summary>
        public void DeletePet()
        {
            ExtentTestManager.CreateTest("Delete pet test");
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("v2/pet/99", Method.Delete);
            var response = client.Execute(request);
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", BaseUrl + "v2/pet/99"));
            ExtentTestManager.TestLogs(string.Concat("Request Url is :- ", (int)response.StatusCode));
            var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That($"{data["message"]}", Is.EqualTo("99"));
        }

        [OneTimeTearDown]
        public void EndTest()
        {
            ExtentTestManager.StatusOfTest();
            ExtentManager.GetExtent().Flush();
        }
    }
}