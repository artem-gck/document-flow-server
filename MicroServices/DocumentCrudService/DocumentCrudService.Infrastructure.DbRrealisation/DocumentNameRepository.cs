﻿using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Domain.Entities;
using DocumentCrudService.Domain.Exceptions;
using DocumentCrudService.Infrastructure.DbRrealisation.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace DocumentCrudService.Infrastructure.DbRrealisation
{
    public class DocumentNameRepository : IDocumentNameRepository
    {
        private readonly DocumentContext _documentContext;

        public DocumentNameRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        }

        public async Task<IEnumerable<DocumentNameEntity>> GetAllAsync()
        {
            var filter = new BsonDocument();

            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);

            var options = new GridFSFindOptions
            {
                Sort = sort
            };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var listOfFileInfo = (await cursor.ToListAsync()).Select(info => new DocumentNameEntity()
            {
                Id = info.Id.ToString(),
                FileName = info.Filename,
                UploadDate = info.UploadDateTime
            });

            return listOfFileInfo;
        }

        public async Task<DocumentEntity> GetByNameAsync(string fileName, int version = -1)
        {
            try
            {
                var options = new GridFSDownloadByNameOptions
                {
                    Revision = version
                };

                var document = await _documentContext.GridFS.DownloadAsBytesByNameAsync(fileName, options);

                var documentEntity = new DocumentEntity()
                {
                    FileName = fileName,
                    File = document
                };

                return documentEntity;
            }
            catch (IndexOutOfRangeException)
            {
                throw new DocumentNotFoundException(fileName);
            }
        }
    }
}
