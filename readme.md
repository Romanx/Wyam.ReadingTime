# Wyam.ReadingTime

[![Build status](https://ci.appveyor.com/api/projects/status/4jufi6k3uwgfmw95/branch/master?svg=true)](https://ci.appveyor.com/project/Romanx/wyam-readingtime/branch/master)
[![Stable Nuget](https://img.shields.io/nuget/v/Wyam.ReadingTime.svg?style=flat-square)](https://www.nuget.org/packages/Wyam.ReadingTime/)

A simple static reading time calculator for plugging into a Wyam pipeline.

Currently uses a very simplistic algorithm and adds metadata object to the pipeline for each post so you can display the information wherever you want on your site.

## Example
```csharp
// Config.wyam

// If you're using the blog recipe then you can use this handy helper
Pipelines.AddReadingTimeMeta(wordsPerMinute: 200);

// If you've setup your own pipeline or know when your content should be processed just add something like the following
Pipelines["BlogPosts"].InsertBefore("WriteMetadata", new ReadingTime(wordsPerMinute: 200));
```

This will add a ReadingTimeMeta object to your context which you can retrieve like:
```csharp
var meta = document.Get<ReadingTimeMeta>(ReadingTimeKeys.ReadingTime);
```

This is currently very rough and allows you to display this information as you like but on my blog I'm doing:
```html
<p class="post-meta">
    Posted on @(post.Get<DateTime>(BlogKeys.Published).ToLongDateString(Context))
    <br />
    Estimated read time:  @(readingMeta.Minutes == 0 ? $"a couple of seconds" : $"{readingMeta.Minutes} minutes") (@(readingMeta.Words) words)
</p>
```