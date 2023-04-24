namespace VipFit.Core.Helpers
{
    using Newtonsoft.Json;

    /// <summary>
    /// Json helper.
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Deserializes json.
        /// </summary>
        /// <typeparam name="T">Object.</typeparam>
        /// <param name="value">Value.</param>
        /// <returns>Deserialized object.</returns>
        public static async Task<T> ToObjectAsync<T>(string value)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(value);
            });
        }

        /// <summary>
        /// Serializes json.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Serialized string.</returns>
        public static async Task<string> StringifyAsync(object value)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.SerializeObject(value);
            });
        }
    }
}