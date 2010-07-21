﻿using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

[MetadataAttribute]
[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
public class PluginInfoAttribute: ExportAttribute
{
    public PluginInfoAttribute() : base(typeof(IPluginBase)) { }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Version { get; set; }
    public string Shortcut { get; set; }
    public string Author { get; set; }
    public string Help { get; set; }
    public string Tags { get; set; }
    public string Bugs { get; set; }
    public string Credits { get; set; }
    public string Warnings { get; set; }
    public int InitialWindowWidth { get; set; }
    public int InitialWindowHeight { get; set; }
    public int InitialBoxWidth { get; set; }
    public int InitialBoxHeight { get; set; }
    public TComponentMode InitialComponentMode { get; set; }
    public bool AutoEvaluate { get; set; }
}
