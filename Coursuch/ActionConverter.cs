using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursuch
{
    class ActionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(AbstractAction).IsAssignableFrom(objectType);
        }

        private AbstractAction Create(Type objectType, JObject jObject)
        {
            if (FieldExists("X", jObject) && FieldExists("Y", jObject) && FieldExists("MouseFlag", jObject))
            {
                return new MouseAction();
            }
            else if (FieldExists("Flag", jObject) && FieldExists("Key", jObject))
            {
                return new KeyBoardAction();
            }
            else {
                return null;
            }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            AbstractAction target = Create(objectType, jObject);

            // Populate the object properties
            if (target != null) serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
