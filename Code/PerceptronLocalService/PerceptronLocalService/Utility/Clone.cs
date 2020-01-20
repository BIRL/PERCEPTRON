using Newtonsoft.Json;

namespace PerceptronLocalService.Utility
{
    public static class Clone
    {
        /// <summary>
        /// USAGE: Some Object that you want to clone (i.e. X1)
        /// Object x=new Object(); //create new
        /// string DeepString = CloneUtility.Clone(X1);
        /// x= CloneUtility.Decrypt&lt;Object&gt;(DeepString);
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        
        public static string CloneObject(object Object)
        {
            var json = JsonConvert.SerializeObject(Object);
            return json;
        }

        public static T Decrypt<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);

        }
    }
}
