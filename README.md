# TplCLI
Tool to create dotnet projects based on templates. Intended to work with [ProjectTemplates](https://github.com/hector-co/ProjectTemplates)

## Usage
When executing ```tpl-cli``` you need to specify where [ProjectTemplates](https://github.com/hector-co/ProjectTemplates) folder is located:


    tpl-cli -p [path-to-project-templates-folder]

It is also possible to specify the project templates folder by default in ```templatesFolder``` key  ```appsettings.yaml``` file.

After that, when running, ```tpl-cli```  will list all available templates and will ask for values that the user needs to provide.

By default ```tpl-cli``` will create the result files in the folder where it is being called, for that reason it is convenient to add the bin directory in the PATH environment variable in windows, or use soft links in linux pointing to the ```tpl-cli``` executable.

## How does it work
```tpl-cli``` will look for folders that contain a file named ```tpl-def.yaml``` and treat that files and folders structure as a templates.

An example ```tpl-def.yaml``` file would be:
```
keys:
  TemplateKey: Template key description
  key2: key2 description
excludedFiles:
  - tpl-def.yaml
excludedFolders:
  - .git
  - bin
  - obj
  - .vs
```

```tpl-cli``` will create a copy of that template (the folder containing the ```tpl-def.yaml``` file) replacing ```'TemplateKey'``` and ```'key2'``` strings in files and folders names, with the values provided by the user.

```excludedFiles``` and ```excludedFolders``` indicates with files and folders are not part of the template, like git folder and generated binaries.

It is possible to create new templates as required, as any folder containing a ```tpl-def.yaml``` will be recognized as a template.