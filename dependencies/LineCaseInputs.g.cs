// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar init'.
// DO NOT EDIT THIS FILE.

using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Elements.Geometry.Solids;
using Elements.Validators;
using Elements.Serialization.JSON;
using Hypar.Functions;
using Hypar.Functions.Execution;
using Hypar.Functions.Execution.AWS;
using Hypar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Elements.Geometry.Line;
using Polygon = Elements.Geometry.Polygon;

namespace LineCase
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public  class LineCaseInputs : S3Args
    
    {
        [Newtonsoft.Json.JsonConstructor]
        
        public LineCaseInputs(Overrides @overrides, string bucketName, string uploadsBucket, Dictionary<string, string> modelInputKeys, string gltfKey, string elementsKey, string ifcKey):
        base(bucketName, uploadsBucket, modelInputKeys, gltfKey, elementsKey, ifcKey)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<LineCaseInputs>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @overrides});
            }
        
            this.Overrides = @overrides ?? this.Overrides;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("overrides", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Overrides Overrides { get; set; } = new Overrides();
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class Overrides 
    
    {
        public Overrides() { }
        
        [Newtonsoft.Json.JsonConstructor]
        public Overrides(OverrideAdditions @additions, OverrideRemovals @removals, IList<GuidesOverride> @guides)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<Overrides>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @additions, @removals, @guides});
            }
        
            this.Additions = @additions ?? this.Additions;
            this.Removals = @removals ?? this.Removals;
            this.Guides = @guides ?? this.Guides;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Additions", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public OverrideAdditions Additions { get; set; } = new OverrideAdditions();
    
        [Newtonsoft.Json.JsonProperty("Removals", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public OverrideRemovals Removals { get; set; } = new OverrideRemovals();
    
        [Newtonsoft.Json.JsonProperty("Guides", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<GuidesOverride> Guides { get; set; } = new List<GuidesOverride>();
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class OverrideAdditions 
    
    {
        public OverrideAdditions() { }
        
        [Newtonsoft.Json.JsonConstructor]
        public OverrideAdditions(IList<GuidesOverrideAddition> @guides)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<OverrideAdditions>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @guides});
            }
        
            this.Guides = @guides ?? this.Guides;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Guides", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<GuidesOverrideAddition> Guides { get; set; } = new List<GuidesOverrideAddition>();
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class OverrideRemovals 
    
    {
        public OverrideRemovals() { }
        
        [Newtonsoft.Json.JsonConstructor]
        public OverrideRemovals(IList<GuidesOverrideRemoval> @guides)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<OverrideRemovals>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @guides});
            }
        
            this.Guides = @guides ?? this.Guides;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Guides", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IList<GuidesOverrideRemoval> Guides { get; set; } = new List<GuidesOverrideRemoval>();
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class GuidesOverride 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public GuidesOverride(string @id, GuidesIdentity @identity, GuidesValue @value)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<GuidesOverride>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @id, @identity, @value});
            }
        
            this.Id = @id;
            this.Identity = @identity;
            this.Value = @value;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Identity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public GuidesIdentity Identity { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public GuidesValue Value { get; set; }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class GuidesOverrideAddition 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public GuidesOverrideAddition(string @id, GuidesIdentity @identity, GuidesOverrideAdditionValue @value)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<GuidesOverrideAddition>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @id, @identity, @value});
            }
        
            this.Id = @id;
            this.Identity = @identity;
            this.Value = @value;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Identity", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public GuidesIdentity Identity { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public GuidesOverrideAdditionValue Value { get; set; }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class GuidesOverrideRemoval 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public GuidesOverrideRemoval(string @id, GuidesIdentity @identity)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<GuidesOverrideRemoval>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @id, @identity});
            }
        
            this.Id = @id;
            this.Identity = @identity;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }
    
        [Newtonsoft.Json.JsonProperty("Identity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public GuidesIdentity Identity { get; set; }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class GuidesIdentity 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public GuidesIdentity(string @addId)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<GuidesIdentity>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @addId});
            }
        
            this.AddId = @addId;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Add Id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AddId { get; set; }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class GuidesValue 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public GuidesValue(Polyline @polyline)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<GuidesValue>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @polyline});
            }
        
            this.Polyline = @polyline;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Polyline", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Polyline Polyline { get; set; }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.21.0 (Newtonsoft.Json v13.0.0.0)")]
    
    public partial class GuidesOverrideAdditionValue 
    
    {
        [Newtonsoft.Json.JsonConstructor]
        public GuidesOverrideAdditionValue(Polyline @polyline)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<GuidesOverrideAdditionValue>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @polyline});
            }
        
            this.Polyline = @polyline;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        [Newtonsoft.Json.JsonProperty("Polyline", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Polyline Polyline { get; set; }
    
    }
}