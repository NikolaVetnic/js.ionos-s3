# IONOS S3 Object Storage in ASP.NET

Implementation of `IBlobStorage` interface suited for IONOS S3 Object Storage.

Prerequisites for running the the test program:
- [IONOS Data Center Designer](https://dcd.ionos.com) access,
- A bucket - Storage / IONOS S3 Object Storage / Buckets / Create bucket,
- Access keys - Storage / IONOS S3 Object Storage / Key management / Generate a key,
- Input the following either into the `appsettings.json` or `appsettings.override.json`:
    - Service URL: Storage / IONOS S3 Object Storage / Buckets / YourBucketName / Bucket Settings / Endpoint URL,
    - Access Key: Storage / IONOS S3 Object Storage / Key management / YourKey / Access Key,
    - Secret Key: Storage / IONOS S3 Object Storage / Key management / YourKey / Secret Key,
    - Bucket name: Storage / IONOS S3 Object Storage / Buckets / YourBucketName