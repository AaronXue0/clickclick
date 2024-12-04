// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mediapipe/tasks/cc/vision/object_detector/proto/object_detector_options.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Mediapipe.Tasks.Vision.ObjectDetector.Proto {

  /// <summary>Holder for reflection information generated from mediapipe/tasks/cc/vision/object_detector/proto/object_detector_options.proto</summary>
  public static partial class ObjectDetectorOptionsReflection {

    #region Descriptor
    /// <summary>File descriptor for mediapipe/tasks/cc/vision/object_detector/proto/object_detector_options.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ObjectDetectorOptionsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ck1tZWRpYXBpcGUvdGFza3MvY2MvdmlzaW9uL29iamVjdF9kZXRlY3Rvci9w",
            "cm90by9vYmplY3RfZGV0ZWN0b3Jfb3B0aW9ucy5wcm90bxIsbWVkaWFwaXBl",
            "LnRhc2tzLnZpc2lvbi5vYmplY3RfZGV0ZWN0b3IucHJvdG8aJG1lZGlhcGlw",
            "ZS9mcmFtZXdvcmsvY2FsY3VsYXRvci5wcm90bxosbWVkaWFwaXBlL2ZyYW1l",
            "d29yay9jYWxjdWxhdG9yX29wdGlvbnMucHJvdG8aMG1lZGlhcGlwZS90YXNr",
            "cy9jYy9jb3JlL3Byb3RvL2Jhc2Vfb3B0aW9ucy5wcm90byLVAgoVT2JqZWN0",
            "RGV0ZWN0b3JPcHRpb25zEj0KDGJhc2Vfb3B0aW9ucxgBIAEoCzInLm1lZGlh",
            "cGlwZS50YXNrcy5jb3JlLnByb3RvLkJhc2VPcHRpb25zEiAKFGRpc3BsYXlf",
            "bmFtZXNfbG9jYWxlGAIgASgJOgJlbhIXCgttYXhfcmVzdWx0cxgDIAEoBToC",
            "LTESFwoPc2NvcmVfdGhyZXNob2xkGAQgASgCEhoKEmNhdGVnb3J5X2FsbG93",
            "bGlzdBgFIAMoCRIZChFjYXRlZ29yeV9kZW55bGlzdBgGIAMoCTJyCgNleHQS",
            "HC5tZWRpYXBpcGUuQ2FsY3VsYXRvck9wdGlvbnMYise50wEgASgLMkMubWVk",
            "aWFwaXBlLnRhc2tzLnZpc2lvbi5vYmplY3RfZGV0ZWN0b3IucHJvdG8uT2Jq",
            "ZWN0RGV0ZWN0b3JPcHRpb25zQlQKNmNvbS5nb29nbGUubWVkaWFwaXBlLnRh",
            "c2tzLnZpc2lvbi5vYmplY3RkZXRlY3Rvci5wcm90b0IaT2JqZWN0RGV0ZWN0",
            "b3JPcHRpb25zUHJvdG8="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Mediapipe.CalculatorReflection.Descriptor, global::Mediapipe.CalculatorOptionsReflection.Descriptor, global::Mediapipe.Tasks.Core.Proto.BaseOptionsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptions), global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptions.Parser, new[]{ "BaseOptions", "DisplayNamesLocale", "MaxResults", "ScoreThreshold", "CategoryAllowlist", "CategoryDenylist" }, null, null, new pb::Extension[] { global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptions.Extensions.Ext }, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ObjectDetectorOptions : pb::IMessage<ObjectDetectorOptions>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ObjectDetectorOptions> _parser = new pb::MessageParser<ObjectDetectorOptions>(() => new ObjectDetectorOptions());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ObjectDetectorOptions> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptionsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ObjectDetectorOptions() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ObjectDetectorOptions(ObjectDetectorOptions other) : this() {
      _hasBits0 = other._hasBits0;
      baseOptions_ = other.baseOptions_ != null ? other.baseOptions_.Clone() : null;
      displayNamesLocale_ = other.displayNamesLocale_;
      maxResults_ = other.maxResults_;
      scoreThreshold_ = other.scoreThreshold_;
      categoryAllowlist_ = other.categoryAllowlist_.Clone();
      categoryDenylist_ = other.categoryDenylist_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ObjectDetectorOptions Clone() {
      return new ObjectDetectorOptions(this);
    }

    /// <summary>Field number for the "base_options" field.</summary>
    public const int BaseOptionsFieldNumber = 1;
    private global::Mediapipe.Tasks.Core.Proto.BaseOptions baseOptions_;
    /// <summary>
    /// Base options for configuring MediaPipe Tasks, such as specifying the TfLite
    /// model file with metadata, accelerator options, etc.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Core.Proto.BaseOptions BaseOptions {
      get { return baseOptions_; }
      set {
        baseOptions_ = value;
      }
    }

    /// <summary>Field number for the "display_names_locale" field.</summary>
    public const int DisplayNamesLocaleFieldNumber = 2;
    private readonly static string DisplayNamesLocaleDefaultValue = global::System.Text.Encoding.UTF8.GetString(global::System.Convert.FromBase64String("ZW4="), 0, 2);

    private string displayNamesLocale_;
    /// <summary>
    /// The locale to use for display names specified through the TFLite Model
    /// Metadata, if any. Defaults to English.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string DisplayNamesLocale {
      get { return displayNamesLocale_ ?? DisplayNamesLocaleDefaultValue; }
      set {
        displayNamesLocale_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "display_names_locale" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasDisplayNamesLocale {
      get { return displayNamesLocale_ != null; }
    }
    /// <summary>Clears the value of the "display_names_locale" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearDisplayNamesLocale() {
      displayNamesLocale_ = null;
    }

    /// <summary>Field number for the "max_results" field.</summary>
    public const int MaxResultsFieldNumber = 3;
    private readonly static int MaxResultsDefaultValue = -1;

    private int maxResults_;
    /// <summary>
    /// The maximum number of top-scored detection results to return. If &lt; 0, all
    /// available results will be returned. If 0, an invalid argument error is
    /// returned. Note that models may intrinsically be limited to returning a
    /// maximum number of results N: if the provided value here is above N, only N
    /// results will be returned.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int MaxResults {
      get { if ((_hasBits0 & 1) != 0) { return maxResults_; } else { return MaxResultsDefaultValue; } }
      set {
        _hasBits0 |= 1;
        maxResults_ = value;
      }
    }
    /// <summary>Gets whether the "max_results" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasMaxResults {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "max_results" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearMaxResults() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "score_threshold" field.</summary>
    public const int ScoreThresholdFieldNumber = 4;
    private readonly static float ScoreThresholdDefaultValue = 0F;

    private float scoreThreshold_;
    /// <summary>
    /// Score threshold to override the one provided in the model metadata (if
    /// any). Detection results with a score below this value are rejected.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float ScoreThreshold {
      get { if ((_hasBits0 & 2) != 0) { return scoreThreshold_; } else { return ScoreThresholdDefaultValue; } }
      set {
        _hasBits0 |= 2;
        scoreThreshold_ = value;
      }
    }
    /// <summary>Gets whether the "score_threshold" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasScoreThreshold {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "score_threshold" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearScoreThreshold() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "category_allowlist" field.</summary>
    public const int CategoryAllowlistFieldNumber = 5;
    private static readonly pb::FieldCodec<string> _repeated_categoryAllowlist_codec
        = pb::FieldCodec.ForString(42);
    private readonly pbc::RepeatedField<string> categoryAllowlist_ = new pbc::RepeatedField<string>();
    /// <summary>
    /// Optional allowlist of category names. If non-empty, detection results whose
    /// category name is not in this set will be filtered out. Duplicate or unknown
    /// category names are ignored. Mutually exclusive with category_denylist.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> CategoryAllowlist {
      get { return categoryAllowlist_; }
    }

    /// <summary>Field number for the "category_denylist" field.</summary>
    public const int CategoryDenylistFieldNumber = 6;
    private static readonly pb::FieldCodec<string> _repeated_categoryDenylist_codec
        = pb::FieldCodec.ForString(50);
    private readonly pbc::RepeatedField<string> categoryDenylist_ = new pbc::RepeatedField<string>();
    /// <summary>
    /// Optional denylist of category names. If non-empty, detection results whose
    /// category name is in this set will be filtered out. Duplicate or unknown
    /// category names are ignored. Mutually exclusive with category_allowlist.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> CategoryDenylist {
      get { return categoryDenylist_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as ObjectDetectorOptions);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(ObjectDetectorOptions other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(BaseOptions, other.BaseOptions)) return false;
      if (DisplayNamesLocale != other.DisplayNamesLocale) return false;
      if (MaxResults != other.MaxResults) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(ScoreThreshold, other.ScoreThreshold)) return false;
      if(!categoryAllowlist_.Equals(other.categoryAllowlist_)) return false;
      if(!categoryDenylist_.Equals(other.categoryDenylist_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (baseOptions_ != null) hash ^= BaseOptions.GetHashCode();
      if (HasDisplayNamesLocale) hash ^= DisplayNamesLocale.GetHashCode();
      if (HasMaxResults) hash ^= MaxResults.GetHashCode();
      if (HasScoreThreshold) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(ScoreThreshold);
      hash ^= categoryAllowlist_.GetHashCode();
      hash ^= categoryDenylist_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (baseOptions_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(BaseOptions);
      }
      if (HasDisplayNamesLocale) {
        output.WriteRawTag(18);
        output.WriteString(DisplayNamesLocale);
      }
      if (HasMaxResults) {
        output.WriteRawTag(24);
        output.WriteInt32(MaxResults);
      }
      if (HasScoreThreshold) {
        output.WriteRawTag(37);
        output.WriteFloat(ScoreThreshold);
      }
      categoryAllowlist_.WriteTo(output, _repeated_categoryAllowlist_codec);
      categoryDenylist_.WriteTo(output, _repeated_categoryDenylist_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (baseOptions_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(BaseOptions);
      }
      if (HasDisplayNamesLocale) {
        output.WriteRawTag(18);
        output.WriteString(DisplayNamesLocale);
      }
      if (HasMaxResults) {
        output.WriteRawTag(24);
        output.WriteInt32(MaxResults);
      }
      if (HasScoreThreshold) {
        output.WriteRawTag(37);
        output.WriteFloat(ScoreThreshold);
      }
      categoryAllowlist_.WriteTo(ref output, _repeated_categoryAllowlist_codec);
      categoryDenylist_.WriteTo(ref output, _repeated_categoryDenylist_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (baseOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BaseOptions);
      }
      if (HasDisplayNamesLocale) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DisplayNamesLocale);
      }
      if (HasMaxResults) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(MaxResults);
      }
      if (HasScoreThreshold) {
        size += 1 + 4;
      }
      size += categoryAllowlist_.CalculateSize(_repeated_categoryAllowlist_codec);
      size += categoryDenylist_.CalculateSize(_repeated_categoryDenylist_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(ObjectDetectorOptions other) {
      if (other == null) {
        return;
      }
      if (other.baseOptions_ != null) {
        if (baseOptions_ == null) {
          BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
        }
        BaseOptions.MergeFrom(other.BaseOptions);
      }
      if (other.HasDisplayNamesLocale) {
        DisplayNamesLocale = other.DisplayNamesLocale;
      }
      if (other.HasMaxResults) {
        MaxResults = other.MaxResults;
      }
      if (other.HasScoreThreshold) {
        ScoreThreshold = other.ScoreThreshold;
      }
      categoryAllowlist_.Add(other.categoryAllowlist_);
      categoryDenylist_.Add(other.categoryDenylist_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (baseOptions_ == null) {
              BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
            }
            input.ReadMessage(BaseOptions);
            break;
          }
          case 18: {
            DisplayNamesLocale = input.ReadString();
            break;
          }
          case 24: {
            MaxResults = input.ReadInt32();
            break;
          }
          case 37: {
            ScoreThreshold = input.ReadFloat();
            break;
          }
          case 42: {
            categoryAllowlist_.AddEntriesFrom(input, _repeated_categoryAllowlist_codec);
            break;
          }
          case 50: {
            categoryDenylist_.AddEntriesFrom(input, _repeated_categoryDenylist_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (baseOptions_ == null) {
              BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
            }
            input.ReadMessage(BaseOptions);
            break;
          }
          case 18: {
            DisplayNamesLocale = input.ReadString();
            break;
          }
          case 24: {
            MaxResults = input.ReadInt32();
            break;
          }
          case 37: {
            ScoreThreshold = input.ReadFloat();
            break;
          }
          case 42: {
            categoryAllowlist_.AddEntriesFrom(ref input, _repeated_categoryAllowlist_codec);
            break;
          }
          case 50: {
            categoryDenylist_.AddEntriesFrom(ref input, _repeated_categoryDenylist_codec);
            break;
          }
        }
      }
    }
    #endif

    #region Extensions
    /// <summary>Container for extensions for other messages declared in the ObjectDetectorOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Extensions {
      public static readonly pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptions> Ext =
        new pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptions>(443442058, pb::FieldCodec.ForMessage(3547536466, global::Mediapipe.Tasks.Vision.ObjectDetector.Proto.ObjectDetectorOptions.Parser));
    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code