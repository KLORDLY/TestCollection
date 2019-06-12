using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumDescriptionTest
{
    public enum CharacterModel
    {
        [EnumDescription("身高")]
        Height,
        [EnumDescription("体重")]
        Weight,
        [EnumDescription("年龄")]
        Age,
    }
    public class TestEnumDescription
    {
        CharacterModel item = CharacterModel.Height;

        public CharacterModel Item
        {
            get { return item; }
            internal set { item = value; }
        }

        public string Text
        {
            get
            {
                return EnumDescription.GetEnumDescription(this.Item);
            }
        }
    }
}
