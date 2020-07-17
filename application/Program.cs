using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace application
{
    class Program
    {
        private const string bucketName = "cdn-orkun-test";
        //private const string keyName = "deneme_folder/alt_resim"; for save file to sub folder (https://forums.aws.amazon.com/thread.jspa?threadID=73964)
        private const string keyName = "keyname"; // display file name and use for get file from url
        private const string filePath = @"c:\orkun\cdn\deneme.png"; // saving file path
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private static IAmazonS3 client;
        static void Main(string[] args)
        {
            client = new AmazonS3Client("<accessKeyId>", "<secretAccessKey>", bucketRegion);

            WritingAnObjectAsync().Wait();
        }

        static async Task WritingAnObjectAsync()
        {
            try
            {
                S3Grant s = new S3Grant();
                s.Permission = S3Permission.READ;

                S3Grantee grantee = new S3Grantee();
                grantee.URI = "http://acs.amazonaws.com/groups/global/AllUsers";//(https://www.cloudconformity.com/knowledge-base/aws/S3/s3-bucket-public-read-access.html)

                s.Grantee = grantee;

                List<S3Grant> list = new List<S3Grant>();
                list.Add(s);

                var putRequest2 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    FilePath = filePath,
                    ContentType = "image/png",
                    Grants = list,

                };

                putRequest2.Metadata.Add("x-amz-meta-title", "someTitle");
                PutObjectResponse response2 = await client.PutObjectAsync(putRequest2);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message: '{0}' when writing an object", e.Message
                );
            }
        }
    }
}
