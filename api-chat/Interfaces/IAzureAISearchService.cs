﻿namespace api_chat.Interfaces
{
    public interface IAzureAISearchService
    {
        Task<string> SimpleVectorSearchAsync(ReadOnlyMemory<float> embedding, string query, string index, int k = 3);
        Task<string> SemanticHybridSearchAsync(ReadOnlyMemory<float> embedding, string query, string index, string semanticconfigname, int k = 3);
    }
}
