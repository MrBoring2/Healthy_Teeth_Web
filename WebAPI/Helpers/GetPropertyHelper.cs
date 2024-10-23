namespace WebAPI.Helpers
{
    public class GetPropertyHelper
    {
        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Значение не может быть null", "src");
            if (propName == null) throw new ArgumentException("Значение не может быть null.", "propName");

            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }
    }
}
