require 'albacore'

BUILD_CONFIGURATION = ENV['BUILD_CONFIGURATION'] || 'Release'

desc 'compile'
msbuild :compile do |msbuild|
  msbuild.solution = 'src/Coypu.sln'
  msbuild.use :net35
  msbuild.properties :configuration => BUILD_CONFIGURATION
end

desc 'test'
nunit :test => :compile do |nunit|
  nunit.command = 'lib\NUnit\nunit-console.exe'
  nunit.assemblies = ['src\\Coypu.Tests\\Coypu.Tests.csproj']
end

desc 'testdrivers'
nunit :testdrivers => :compile do |nunit|
  nunit.command = 'lib\nspec\NSpecRunner.exe'
  nunit.assemblies = ["src\\Coypu.Drivers.Tests\\bin\\#{BUILD_CONFIGURATION.downcase}\\Coypu.Drivers.Tests.dll"]
end
