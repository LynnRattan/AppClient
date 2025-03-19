using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AppClient.Models;
using AppClient.ModelsExt;
//using Windows.Media.Protection.PlayReady;
//using Windows.Media.Protection.PlayReady;

namespace AppClient.Services
{
    public class LMBWebApi
    {
        #region without tunnel

        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "localhost";
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5039/api/" : $"http://{serverIP}:5039/api/";
        public static string ImageBaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5039" : $"http://{serverIP}:5039";

        #endregion

        #region with tunnel
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        //private static string serverIP = "6gh41sf3-5039.euw.devtunnels.ms";
        //private HttpClient client;
        //private string baseUrl;
        //public static string BaseAddress = "https://6gh41sf3-5039.euw.devtunnels.ms/api/";
        //public static string ImageBaseAddress = "https://6gh41sf3-5039.euw.devtunnels.ms/";
        #endregion

        public LMBWebApi()
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            this.client = new HttpClient(handler);
            this.baseUrl = BaseAddress;
        }

        public string GetImagesBaseAddress()
        {
            return LMBWebApi.ImageBaseAddress;
        }

        public string GetDefaultProfilePhotoUrl()
        {
            return $"{LMBWebApi.ImageBaseAddress}/profileImages/default.png";
        }
        public string GetDefaultDessertPhotoUrl()
        {
            return $"{LMBWebApi.ImageBaseAddress}/dessertImages/defaultD.png";
        }
        public async Task<User?> LoginAsync(LoginInfo userInfo)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}login";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(userInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    User? result = JsonSerializer.Deserialize<User>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //This methos call the Register web API on the server and return the AppUser object with the given ID
        //or null if the call fails
        public async Task<UserBaker?> SignUp(UserBaker user)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}signup";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    UserBaker? result = JsonSerializer.Deserialize<UserBaker>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //This methos call the Register web API on the server and return the AppUser object with the given ID
        //or null if the call fails
        //public async Task<Baker?> SignUp(Baker baker)
        //{
        //    //Set URI to the specific function API
        //    string url = $"{this.baseUrl}bakersignup";
        //    try
        //    {
        //        //Call the server API
        //        string json = JsonSerializer.Serialize(baker);
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(url, content);
        //        //Check status
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //Extract the content as string
        //            string resContent = await response.Content.ReadAsStringAsync();
        //            //Desrialize result
        //            JsonSerializerOptions options = new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            };
        //            Baker? result = JsonSerializer.Deserialize<Baker>(resContent, options);
        //            return result;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //This method call the UploadProfileImage web API on the server and return the AppUser object with the given URL
        //of the profile image or null if the call fails
        //when registering a user it is better first to call the register command and right after that call this function
        public async Task<User?> UploadProfileImage(string imagePath)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UploadProfileImage";
            try
            {
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    User? result = JsonSerializer.Deserialize<User>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Same operation for dessert
        public async Task<Dessert?> UploadDessertImage(string imagePath, int dessertId, int userId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UploadDessertImage?dessertId={dessertId}&userId={userId}";

            try
            {
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Dessert? result = JsonSerializer.Deserialize<Dessert>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Dessert?> UpdateDessertImage(Dessert dessert)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}updateDessertImage";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(dessert);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Dessert? result = JsonSerializer.Deserialize<Dessert>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //This methos call the Register web API on the server and return the AppUser object with the given ID
        //or null if the call fails
        public async Task<Dessert?> AddDessert(Dessert d)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}adddessert";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(d);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Dessert? result = JsonSerializer.Deserialize<Dessert>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public async Task<List<Baker>?> GetBakers()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getbakers";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<Baker>? result = JsonSerializer.Deserialize<List<Baker>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async void ApproveCon(int bakerId)
        {
            string url = $"{this.baseUrl}approvebaker";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(bakerId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async void DeclineCon(int bakerId)
        {
            string url = $"{this.baseUrl}declinebaker";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(bakerId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<List<Dessert>?> GetDesserts()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getdesserts";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<Dessert>? result = JsonSerializer.Deserialize<List<Dessert>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async void ApproveDes(int dessertId)
        {
            string url = $"{this.baseUrl}approvedessert";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(dessertId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async void DeclineDes(int dessertId)
        {
            string url = $"{this.baseUrl}declinedessert";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(dessertId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //This method call the getConfectioneryTypes web API and return a list of UrgencyLevel or null if it fails.
        public async Task<List<ConfectioneryType>?> GetConfectioneryTypes()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getconfectionerytypes";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<ConfectioneryType>? result = JsonSerializer.Deserialize<List<ConfectioneryType>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //This method call the getDessertTypes web API and return a list of UrgencyLevel or null if it fails.
        public async Task<List<DessertType>?> GetDessertTypes()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getdesserttypes";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<DessertType>? result = JsonSerializer.Deserialize<List<DessertType>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Status>?> GetStatuses()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getstatusestypes";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<Status>? result = JsonSerializer.Deserialize<List<Status>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Dessert>?> GetBakerDesserts(int bakerId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getbakerdesserts";
            try
            {
                string json = JsonSerializer.Serialize(bakerId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<Dessert>? result = JsonSerializer.Deserialize<List<Dessert>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Baker?> GetBaker(int bakerId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getbaker";
            try
            {
                string json = JsonSerializer.Serialize(bakerId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Baker? result = JsonSerializer.Deserialize<Baker>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void UpdateHighestPrice(Baker baker)
        {
            string url = $"{this.baseUrl}updateHighestPrice";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(baker);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<List<User>?> GetUsers()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getusers";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<User>? result = JsonSerializer.Deserialize<List<User>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OrderedDessert?> AddOrderedDessert(OrderedDessert d)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}addordereddessert";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(d);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    OrderedDessert? result = JsonSerializer.Deserialize<OrderedDessert>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<OrderedDessert>?> GetOrderedDesserts()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getordereddesserts";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<OrderedDessert>? result = JsonSerializer.Deserialize<List<OrderedDessert>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteOD(int dessertId)
        {
            string parameterKey = "id";
            string parameterValue = dessertId.ToString();
            string url = $"{this.baseUrl}DeleteOrderedDessert?{parameterKey}={parameterValue}";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<OrderedDessert?> UpdateQuantity(OrderedDessert d, int quantity)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UpdateQuantity?quantity={quantity}";

            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(d);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    OrderedDessert? result = JsonSerializer.Deserialize<OrderedDessert>(resContent, options);
                    return result;
                }
                else
                {
                    return null; ;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order?> AddOrder(Order o)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}addorder";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(o);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Order? result = JsonSerializer.Deserialize<Order>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async void PutInOrder(int orderedDessertId, int orderId)
        {
            string url = $"{this.baseUrl}PutInOrder?orderId={orderId}";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(orderedDessertId);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<List<Order>?> GetOrders()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getorders";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<Order>? result = JsonSerializer.Deserialize<List<Order>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async void UpdateProfits(Baker b)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UpdateProfits";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(b);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }

        public async void DeclineOrderedDes(int id)
        {
            string url = $"{this.baseUrl}DeclineOrderedDessert";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(id);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async void ApproveOrderedDes(int id)
        {
            string url = $"{this.baseUrl}ApproveOrderedDessert";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(id);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async void DeclineOrder(int id)
        {
            string url = $"{this.baseUrl}declineorder";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(id);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async void ApproveOrder(int id,DateOnly arrivalDate)
        {
            string url = $"{this.baseUrl}approveorder?arrivalDate={arrivalDate}";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(id);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<OrderedDessert?> UpdateTotalPrice(Order o, double newPrice)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UpdateTotalPrice?newPrice={newPrice}";

            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(o);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    OrderedDessert? result = JsonSerializer.Deserialize<OrderedDessert>(resContent, options);
                    return result;
                }
                else
                {
                    return null; ;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteFromMenu(int dessertId)
        {
            string parameterKey = "id";
            string parameterValue = dessertId.ToString();
            string url = $"{this.baseUrl}DeleteFromMenu?{parameterKey}={parameterValue}";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Extract the content as string
                string resContent = await response.Content.ReadAsStringAsync();
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

