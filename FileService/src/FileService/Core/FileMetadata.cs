﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Core;

public class FileMetadata
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("name")] public string Name { get; set; } = string.Empty;

    [BsonElement("content_type")] public string ContentType { get; set; } = string.Empty;

    public string FullPath => $"{BucketName}/{Key}";

    [BsonElement("size")] public long Size { get; set; }

    public string StorageInfo { get; set; } = string.Empty;

    [BsonElement("bucket_name")] public string BucketName { get; set; } = string.Empty;

    [BsonElement("prefix")] public string Prefix { get; set; } = string.Empty;

    [BsonElement("key")] public string Key { get; set; } = string.Empty;

    public string UploadId { get; set; } = string.Empty;

    [BsonElement("download_url")] public string DownloadUrl { get; set; } = string.Empty;

    public int PartNumber { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public IEnumerable<ETagInfo>? ETags;
}