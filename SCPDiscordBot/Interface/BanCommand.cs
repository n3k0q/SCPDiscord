// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: BotToPlugin/BanCommand.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SCPDiscord.Interface {

  /// <summary>Holder for reflection information generated from BotToPlugin/BanCommand.proto</summary>
  public static partial class BanCommandReflection {

    #region Descriptor
    /// <summary>File descriptor for BotToPlugin/BanCommand.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BanCommandReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxCb3RUb1BsdWdpbi9CYW5Db21tYW5kLnByb3RvEhRTQ1BEaXNjb3JkLklu",
            "dGVyZmFjZSKVAQoKQmFuQ29tbWFuZBIRCgljaGFubmVsSUQYASABKAQSDwoH",
            "c3RlYW1JRBgCIAEoCRIQCghkdXJhdGlvbhgDIAEoCRIOCgZyZWFzb24YBCAB",
            "KAkSEAoIYWRtaW5UYWcYBSABKAkSFQoNaW50ZXJhY3Rpb25JRBgGIAEoBBIY",
            "ChBpbnRlcmFjdGlvblRva2VuGAcgASgJYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SCPDiscord.Interface.BanCommand), global::SCPDiscord.Interface.BanCommand.Parser, new[]{ "ChannelID", "SteamID", "Duration", "Reason", "AdminTag", "InteractionID", "InteractionToken" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class BanCommand : pb::IMessage<BanCommand>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<BanCommand> _parser = new pb::MessageParser<BanCommand>(() => new BanCommand());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<BanCommand> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SCPDiscord.Interface.BanCommandReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BanCommand() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BanCommand(BanCommand other) : this() {
      channelID_ = other.channelID_;
      steamID_ = other.steamID_;
      duration_ = other.duration_;
      reason_ = other.reason_;
      adminTag_ = other.adminTag_;
      interactionID_ = other.interactionID_;
      interactionToken_ = other.interactionToken_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public BanCommand Clone() {
      return new BanCommand(this);
    }

    /// <summary>Field number for the "channelID" field.</summary>
    public const int ChannelIDFieldNumber = 1;
    private ulong channelID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong ChannelID {
      get { return channelID_; }
      set {
        channelID_ = value;
      }
    }

    /// <summary>Field number for the "steamID" field.</summary>
    public const int SteamIDFieldNumber = 2;
    private string steamID_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string SteamID {
      get { return steamID_; }
      set {
        steamID_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "duration" field.</summary>
    public const int DurationFieldNumber = 3;
    private string duration_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Duration {
      get { return duration_; }
      set {
        duration_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "reason" field.</summary>
    public const int ReasonFieldNumber = 4;
    private string reason_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Reason {
      get { return reason_; }
      set {
        reason_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "adminTag" field.</summary>
    public const int AdminTagFieldNumber = 5;
    private string adminTag_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string AdminTag {
      get { return adminTag_; }
      set {
        adminTag_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "interactionID" field.</summary>
    public const int InteractionIDFieldNumber = 6;
    private ulong interactionID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong InteractionID {
      get { return interactionID_; }
      set {
        interactionID_ = value;
      }
    }

    /// <summary>Field number for the "interactionToken" field.</summary>
    public const int InteractionTokenFieldNumber = 7;
    private string interactionToken_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string InteractionToken {
      get { return interactionToken_; }
      set {
        interactionToken_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as BanCommand);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(BanCommand other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ChannelID != other.ChannelID) return false;
      if (SteamID != other.SteamID) return false;
      if (Duration != other.Duration) return false;
      if (Reason != other.Reason) return false;
      if (AdminTag != other.AdminTag) return false;
      if (InteractionID != other.InteractionID) return false;
      if (InteractionToken != other.InteractionToken) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ChannelID != 0UL) hash ^= ChannelID.GetHashCode();
      if (SteamID.Length != 0) hash ^= SteamID.GetHashCode();
      if (Duration.Length != 0) hash ^= Duration.GetHashCode();
      if (Reason.Length != 0) hash ^= Reason.GetHashCode();
      if (AdminTag.Length != 0) hash ^= AdminTag.GetHashCode();
      if (InteractionID != 0UL) hash ^= InteractionID.GetHashCode();
      if (InteractionToken.Length != 0) hash ^= InteractionToken.GetHashCode();
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
      if (ChannelID != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(ChannelID);
      }
      if (SteamID.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(SteamID);
      }
      if (Duration.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Duration);
      }
      if (Reason.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Reason);
      }
      if (AdminTag.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(AdminTag);
      }
      if (InteractionID != 0UL) {
        output.WriteRawTag(48);
        output.WriteUInt64(InteractionID);
      }
      if (InteractionToken.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(InteractionToken);
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
      if (ChannelID != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(ChannelID);
      }
      if (SteamID.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(SteamID);
      }
      if (Duration.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Duration);
      }
      if (Reason.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Reason);
      }
      if (AdminTag.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(AdminTag);
      }
      if (InteractionID != 0UL) {
        output.WriteRawTag(48);
        output.WriteUInt64(InteractionID);
      }
      if (InteractionToken.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(InteractionToken);
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
      if (ChannelID != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(ChannelID);
      }
      if (SteamID.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SteamID);
      }
      if (Duration.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Duration);
      }
      if (Reason.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Reason);
      }
      if (AdminTag.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AdminTag);
      }
      if (InteractionID != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(InteractionID);
      }
      if (InteractionToken.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(InteractionToken);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(BanCommand other) {
      if (other == null) {
        return;
      }
      if (other.ChannelID != 0UL) {
        ChannelID = other.ChannelID;
      }
      if (other.SteamID.Length != 0) {
        SteamID = other.SteamID;
      }
      if (other.Duration.Length != 0) {
        Duration = other.Duration;
      }
      if (other.Reason.Length != 0) {
        Reason = other.Reason;
      }
      if (other.AdminTag.Length != 0) {
        AdminTag = other.AdminTag;
      }
      if (other.InteractionID != 0UL) {
        InteractionID = other.InteractionID;
      }
      if (other.InteractionToken.Length != 0) {
        InteractionToken = other.InteractionToken;
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
            ChannelID = input.ReadUInt64();
            break;
          }
          case 18: {
            SteamID = input.ReadString();
            break;
          }
          case 26: {
            Duration = input.ReadString();
            break;
          }
          case 34: {
            Reason = input.ReadString();
            break;
          }
          case 42: {
            AdminTag = input.ReadString();
            break;
          }
          case 48: {
            InteractionID = input.ReadUInt64();
            break;
          }
          case 58: {
            InteractionToken = input.ReadString();
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
            ChannelID = input.ReadUInt64();
            break;
          }
          case 18: {
            SteamID = input.ReadString();
            break;
          }
          case 26: {
            Duration = input.ReadString();
            break;
          }
          case 34: {
            Reason = input.ReadString();
            break;
          }
          case 42: {
            AdminTag = input.ReadString();
            break;
          }
          case 48: {
            InteractionID = input.ReadUInt64();
            break;
          }
          case 58: {
            InteractionToken = input.ReadString();
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
