// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Card_Base.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace CardBase {

  /// <summary>Holder for reflection information generated from Card_Base.proto</summary>
  public static partial class CardBaseReflection {

    #region Descriptor
    /// <summary>File descriptor for Card_Base.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CardBaseReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg9DYXJkX0Jhc2UucHJvdG8SCUNhcmRfQmFzZSJTCglDYXJkX0Jhc2USCgoC",
            "SUQYASABKAUSDAoETmFtZRgCIAEoCRIPCgdxdWFsaXR5GAMgASgREgwKBElj",
            "b24YBCABKAkSDQoFU3RvcnkYBSABKAkitgEKD0NhcmRfQmFzZV9kYXRhcxI4",
            "CgdkYXRhbWFwGAEgAygLMicuQ2FyZF9CYXNlLkNhcmRfQmFzZV9kYXRhcy5E",
            "YXRhbWFwRW50cnkSIwoFZGF0YXMYAiADKAsyFC5DYXJkX0Jhc2UuQ2FyZF9C",
            "YXNlGkQKDERhdGFtYXBFbnRyeRILCgNrZXkYASABKAUSIwoFdmFsdWUYAiAB",
            "KAsyFC5DYXJkX0Jhc2UuQ2FyZF9CYXNlOgI4AWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::CardBase.Card_Base), global::CardBase.Card_Base.Parser, new[]{ "ID", "Name", "Quality", "Icon", "Story" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::CardBase.Card_Base_datas), global::CardBase.Card_Base_datas.Parser, new[]{ "Datamap", "Datas" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Card_Base : pb::IMessage<Card_Base>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Card_Base> _parser = new pb::MessageParser<Card_Base>(() => new Card_Base());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<Card_Base> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::CardBase.CardBaseReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Card_Base() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Card_Base(Card_Base other) : this() {
      iD_ = other.iD_;
      name_ = other.name_;
      quality_ = other.quality_;
      icon_ = other.icon_;
      story_ = other.story_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Card_Base Clone() {
      return new Card_Base(this);
    }

    /// <summary>Field number for the "ID" field.</summary>
    public const int IDFieldNumber = 1;
    private int iD_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ID {
      get { return iD_; }
      set {
        iD_ = value;
      }
    }

    /// <summary>Field number for the "Name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "quality" field.</summary>
    public const int QualityFieldNumber = 3;
    private int quality_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Quality {
      get { return quality_; }
      set {
        quality_ = value;
      }
    }

    /// <summary>Field number for the "Icon" field.</summary>
    public const int IconFieldNumber = 4;
    private string icon_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Icon {
      get { return icon_; }
      set {
        icon_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Story" field.</summary>
    public const int StoryFieldNumber = 5;
    private string story_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Story {
      get { return story_; }
      set {
        story_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as Card_Base);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(Card_Base other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ID != other.ID) return false;
      if (Name != other.Name) return false;
      if (Quality != other.Quality) return false;
      if (Icon != other.Icon) return false;
      if (Story != other.Story) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ID != 0) hash ^= ID.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Quality != 0) hash ^= Quality.GetHashCode();
      if (Icon.Length != 0) hash ^= Icon.GetHashCode();
      if (Story.Length != 0) hash ^= Story.GetHashCode();
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
      if (ID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ID);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (Quality != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Quality);
      }
      if (Icon.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Icon);
      }
      if (Story.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Story);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ID);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (Quality != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Quality);
      }
      if (Icon.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Icon);
      }
      if (Story.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Story);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (ID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ID);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Quality != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Quality);
      }
      if (Icon.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Icon);
      }
      if (Story.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Story);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(Card_Base other) {
      if (other == null) {
        return;
      }
      if (other.ID != 0) {
        ID = other.ID;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Quality != 0) {
        Quality = other.Quality;
      }
      if (other.Icon.Length != 0) {
        Icon = other.Icon;
      }
      if (other.Story.Length != 0) {
        Story = other.Story;
      }
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
          case 8: {
            ID = input.ReadInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 24: {
            Quality = input.ReadSInt32();
            break;
          }
          case 34: {
            Icon = input.ReadString();
            break;
          }
          case 42: {
            Story = input.ReadString();
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
          case 8: {
            ID = input.ReadInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 24: {
            Quality = input.ReadSInt32();
            break;
          }
          case 34: {
            Icon = input.ReadString();
            break;
          }
          case 42: {
            Story = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class Card_Base_datas : pb::IMessage<Card_Base_datas>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<Card_Base_datas> _parser = new pb::MessageParser<Card_Base_datas>(() => new Card_Base_datas());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<Card_Base_datas> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::CardBase.CardBaseReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Card_Base_datas() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Card_Base_datas(Card_Base_datas other) : this() {
      datamap_ = other.datamap_.Clone();
      datas_ = other.datas_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public Card_Base_datas Clone() {
      return new Card_Base_datas(this);
    }

    /// <summary>Field number for the "datamap" field.</summary>
    public const int DatamapFieldNumber = 1;
    private static readonly pbc::MapField<int, global::CardBase.Card_Base>.Codec _map_datamap_codec
        = new pbc::MapField<int, global::CardBase.Card_Base>.Codec(pb::FieldCodec.ForInt32(8, 0), pb::FieldCodec.ForMessage(18, global::CardBase.Card_Base.Parser), 10);
    private readonly pbc::MapField<int, global::CardBase.Card_Base> datamap_ = new pbc::MapField<int, global::CardBase.Card_Base>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::MapField<int, global::CardBase.Card_Base> Datamap {
      get { return datamap_; }
    }

    /// <summary>Field number for the "datas" field.</summary>
    public const int DatasFieldNumber = 2;
    private static readonly pb::FieldCodec<global::CardBase.Card_Base> _repeated_datas_codec
        = pb::FieldCodec.ForMessage(18, global::CardBase.Card_Base.Parser);
    private readonly pbc::RepeatedField<global::CardBase.Card_Base> datas_ = new pbc::RepeatedField<global::CardBase.Card_Base>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::CardBase.Card_Base> Datas {
      get { return datas_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as Card_Base_datas);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(Card_Base_datas other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!Datamap.Equals(other.Datamap)) return false;
      if(!datas_.Equals(other.datas_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= Datamap.GetHashCode();
      hash ^= datas_.GetHashCode();
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
      datamap_.WriteTo(output, _map_datamap_codec);
      datas_.WriteTo(output, _repeated_datas_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      datamap_.WriteTo(ref output, _map_datamap_codec);
      datas_.WriteTo(ref output, _repeated_datas_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      size += datamap_.CalculateSize(_map_datamap_codec);
      size += datas_.CalculateSize(_repeated_datas_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(Card_Base_datas other) {
      if (other == null) {
        return;
      }
      datamap_.MergeFrom(other.datamap_);
      datas_.Add(other.datas_);
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
            datamap_.AddEntriesFrom(input, _map_datamap_codec);
            break;
          }
          case 18: {
            datas_.AddEntriesFrom(input, _repeated_datas_codec);
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
            datamap_.AddEntriesFrom(ref input, _map_datamap_codec);
            break;
          }
          case 18: {
            datas_.AddEntriesFrom(ref input, _repeated_datas_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
