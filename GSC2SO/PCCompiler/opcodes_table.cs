﻿namespace GameScriptCompiler_v3_pc
{
    public partial class ScriptCompiler
    {
        public const byte OP_End = 0;
        public const byte OP_Return = 0x1;
        public const byte OP_GetUndefined = 0x2;
        public const byte OP_GetZero = 0x3;
        public const byte OP_GetByte = 0x4;
        public const byte OP_GetNegByte = 0x5;
        public const byte OP_GetUnsignedShort = 0x6;
        public const byte OP_GetNegUnsignedShort = 0x7;
        public const byte OP_GetInteger = 0x8;
        public const byte OP_GetFloat = 0x9;
        public const byte OP_GetString = 0xA;
        public const byte OP_GetIString = 0xB;
        public const byte OP_GetVector = 0xC;
        public const byte OP_GetLevelObject = 0xD;
        public const byte OP_GetAnimObject = 0xE;
        public const byte OP_GetSelf = 0xF;
        public const byte OP_GetLevel = 0x10;
        public const byte OP_GetGame = 0x11;
        public const byte OP_GetAnim = 0x12;
        public const byte OP_GetAnimation = 0x13;
        public const byte OP_GetGameRef = 0x14;
        public const byte OP_GetFunction = 0x15;
        public const byte OP_CreateLocalVariables = 0x17;
        public const byte OP_EvalLocalVariableCached = 0x19;
        public const byte OP_EvalArray = 0x1A;
        public const byte OP_EvalArrayRef = 0x1C;
        public const byte OP_ClearArray = 0x1D;
        public const byte OP_EmptyArray = 0x1E;
        public const byte OP_GetSelfObject = 0x1F;
        public const byte OP_EvalFieldVariable = 0x20;
        public const byte OP_EvalFieldVariableRef = 0x21;
        public const byte OP_ClearFieldVariable = 0x22;
        public const byte OP_SafeSetWaittillVariableFieldCached = 0x24;
        public const byte OP_clearparams = 0x25;
        public const byte OP_checkclearparams = 0x26;
        public const byte OP_EvalLocalVariableRefCached = 0x27;
        public const byte OP_SetVariableField = 0x28;
        public const byte OP_wait = 0x2B;
        public const byte OP_waittillFrameEnd = 0x2C;
        public const byte OP_PreScriptCall = 0x2D;
        public const byte OP_ScriptFunctionCall = 0x2E;
        public const byte OP_ScriptFunctionCallPointer = 0x2F;
        public const byte OP_ScriptMethodCall = 0x30;
        public const byte OP_ScriptMethodCallPointer = 0x31;
        public const byte OP_ScriptThreadCall = 0x32;
        public const byte OP_ScriptThreadCallPointer = 0x33;
        public const byte OP_ScriptMethodThreadCall = 0x34;
        public const byte OP_ScriptMethodThreadCallPointer = 0x35;
        public const byte OP_DecTop = 0x36;
        public const byte OP_CastFieldObject = 0x37;
        public const byte OP_CastBool = 0x38;
        public const byte OP_BoolNot = 0x39;
        public const byte OP_BoolComplement = 0x3A;
        public const byte OP_JumpOnFalse = 0x3B;
        public const byte OP_JumpOnTrue = 0x3C;
        public const byte OP_JumpOnFalseExpr = 0x3D;
        public const byte OP_JumpOnTrueExpr = 0x3E;
        public const byte OP_jump = 0x3F;
        public const byte OP_jumpback = 0x40;
        public const byte OP_inc = 0x41;
        public const byte OP_dec = 0x42;
        public const byte OP_bit_or = 0x43;
        public const byte OP_bit_ex_or = 0x44;
        public const byte OP_bit_and = 0x45;
        public const byte OP_equality = 0x46;
        public const byte OP_inequality = 0x47;
        public const byte OP_less = 0x48;
        public const byte OP_greater = 0x49;
        public const byte OP_less_equal = 0x4A;
        public const byte OP_greater_equal = 0x4B;
        public const byte OP_shift_left = 0x4C;
        public const byte OP_shift_right = 0x4D;
        public const byte OP_plus = 0x4E;
        public const byte OP_minus = 0x4F;
        public const byte OP_multiply = 0x50;
        public const byte OP_divide = 0x51;
        public const byte OP_mod = 0x52;
        public const byte OP_size = 0x53;
        public const byte OP_waittillmatch = 0x54;
        public const byte OP_waittill = 0x55;
        public const byte OP_notify = 0x56;
        public const byte OP_endon = 0x57;
        public const byte OP_voidCodepos = 0x58;
        public const byte OP_switch = 0x59;
        public const byte OP_endswitch = 0x5A;
        public const byte OP_vector = 0x5B;
        public const byte OP_GetHash = 0x5C;
        public const byte OP_GetSimpleVector = 0x5E;
        public const byte OP_isdefined = 0x5F;
        public const byte OP_vectorscale = 0x60;
        public const byte OP_anglestoup = 0x61;
        public const byte OP_anglestoright = 0x62;
        public const byte OP_anglestoforward = 0x63;
        public const byte OP_angleclamp180 = 0x64;
        public const byte OP_vectortoangles = 0x65;
        public const byte OP_abs = 0x66;
        public const byte OP_gettime = 0x67;
        public const byte OP_getdvar = 0x68;
        public const byte OP_getdvarint = 0x69;
        public const byte OP_getdvarfloat = 0x6A;
        public const byte OP_GetFirstArrayKey = 0x70;
        public const byte OP_GetNextArrayKey = 0x71;
        public const byte OP_GetUndefined2 = 0x73;
        public const byte OP_skipdev = 0x7B;
    }
}