// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Game.Data
{
    using System;
    using System.Collections.Generic;
    using Game.Data;
    
    
    public class hero : LocalDataBase
    {
        
        private string _Name;
        
        private string _Level;
        
        private double _StarLevel;
        
        private double _NextLevel;
        
        private System.Collections.Generic.List<string> _AttributeName;
        
        private System.Collections.Generic.List<double> _AttributeValue;
        
        private System.Collections.Generic.List<double> _Skills;
        
        // 名称
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        
        // 级别
        public string Level
        {
            get
            {
                return this._Level;
            }
            set
            {
                this._Level = value;
            }
        }
        
        // 星级
        public double StarLevel
        {
            get
            {
                return this._StarLevel;
            }
            set
            {
                this._StarLevel = value;
            }
        }
        
        // 下个等级
        public double NextLevel
        {
            get
            {
                return this._NextLevel;
            }
            set
            {
                this._NextLevel = value;
            }
        }
        
        // 属性名
        public System.Collections.Generic.List<string> AttributeName
        {
            get
            {
                return this._AttributeName;
            }
            set
            {
                this._AttributeName = value;
            }
        }
        
        // 属性值
        public System.Collections.Generic.List<double> AttributeValue
        {
            get
            {
                return this._AttributeValue;
            }
            set
            {
                this._AttributeValue = value;
            }
        }
        
        // 拥有技能id
        public System.Collections.Generic.List<double> Skills
        {
            get
            {
                return this._Skills;
            }
            set
            {
                this._Skills = value;
            }
        }
    }
}
