![VMWare](http://www.codeproject.com/KB/library/VMWareTasks/VMWareLogo.jpg)Vix.Net
=======
This is a `.NET` wrapper over the `VMWare VIX API` based on the open source code from this [CodeProject page](http://www.codeproject.com/Articles/31961/Automating-VMWare-Tasks-in-C-with-the-VIX-API).

I tried to improve this library adding async/await Task supporting and making NuGet-package for it.

Background
----------
There're two types of VMWare APIs.

* VMWare Virtual Infrastructure SDK: a set of tools and APIs to manage a VMWare Infrastructure environment. A toolkit has also been released that contains managed wrappers on top of the SOAP interface provided by a VMWare deployment. It's focused on VMWare ESX or VirtualCenter management and is beyond the scope of this article.
* VMWare VIX API. The VIX API allows developers to write programs and scripts that automate virtual machine operations, as well as the guests within virtual machines. It runs on both Windows and Linux and supports management of VMware Server, Workstation, and Virtual Infrastructure (both ESX and vCenter). Bindings are provided for C, Perl, and COM (Visual Basic, VBscript, C#).

More information you can see on the [CodeProject page](http://www.codeproject.com/Articles/31961/Automating-VMWare-Tasks-in-C-with-the-VIX-API). 




