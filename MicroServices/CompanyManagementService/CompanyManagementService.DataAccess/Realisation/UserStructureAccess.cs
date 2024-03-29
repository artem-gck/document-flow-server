﻿using CompanyManagementService.DataAccess.Exceptions;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CompanyManagementService.DataAccess.Realisation
{
    public class UserStructureAccess : IUserStructureAccess
    {
        private readonly HttpClient _httpClient;

        public UserStructureAccess(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task DeleteAsync(Guid departmentId, Guid userId)
        {
            var answer = await _httpClient.DeleteAsync($"{departmentId}/users/{userId}");

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(userId),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserStructureAccess))
            };
        }

        public async Task<UserResponce> GetAsync(Guid departmentId, Guid userId)
         => await GetAsync(departmentId, userId, null);

        public async Task<UserResponce> GetAsync(Guid departmentId, Guid userId, string token)
        {
            if (!string.IsNullOrWhiteSpace(token) && !_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var answer = await _httpClient.GetAsync($"{departmentId}/users/{userId}");

            if (answer.IsSuccessStatusCode)
            {
                var userResponce = await answer.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserResponce>(userResponce);

                return user;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(userId),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserStructureAccess))
            };
        }

        public async Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id)
            => await GetByDepartmentIdAsync(id, null);

        public async Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id, string token)
        {
            if (!string.IsNullOrWhiteSpace(token) && !_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var answer = await _httpClient.GetAsync($"{id}/users");

            if (answer.IsSuccessStatusCode)
            {
                var userResponce = await answer.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<UserResponce>>(userResponce);

                return users;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(id),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserStructureAccess))
            };
        }

        public async Task<Guid> PostAsync(Guid departmentId, AddUserRequest addUserRequest)
        {
            var answer = await _httpClient.PostAsJsonAsync($"{departmentId}/users", addUserRequest);

            if (answer.IsSuccessStatusCode)
            {
                var idResponce = await answer.Content.ReadAsStringAsync();
                var id = Guid.Parse(idResponce);

                return id;
            }

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(addUserRequest)),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserStructureAccess))
            };
        }

        public async Task PutAsync(Guid departmentId, Guid userId, UpdateUserRequest updateUserRequest)
        {
            var answer = await _httpClient.PutAsJsonAsync($"{departmentId}/users/{userId}", updateUserRequest);

            if (answer.IsSuccessStatusCode)
                return;

            throw answer.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidModelStateException(nameof(updateUserRequest)),
                HttpStatusCode.NotFound => new NotFoundException(userId),
                HttpStatusCode.Conflict => new DbUpdateException(),
                HttpStatusCode.InternalServerError => new InternalServerException(nameof(UserStructureAccess))
            };
        }
    }
}
