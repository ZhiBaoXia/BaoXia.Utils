using System;

namespace BaoXia.Utils.Attributes
{
        /// <summary>
        /// 描述属性，用于标记某些属性的描述信息。
        /// </summary>
        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class DescriptionAttribute : Attribute
        {
                public string Name { get; set; }

                public string? Description { get; set; }

                public DescriptionAttribute(
                        string name,
                        string? description = null)
                {
                        this.Name = name;

                        this.Description = description;
                }
        }
}
