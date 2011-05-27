require 'albacore'

BUILD_CONFIGURATION = ENV['BUILD_CONFIGURATION'] || 'Release'

task :default => :compile
task :compile => :compile_net35

[:net35, :net40].each do |version|
  msbuild "compile_#{version.to_s}".to_sym do |msbuild|
    msbuild.solution = 'src/Coypu.sln'
    msbuild.use version
    msbuild.properties :configuration => BUILD_CONFIGURATION
  end
end

desc 'test'
nunit :test => :compile do |nunit|
  nunit.command = 'lib\NUnit\nunit-console.exe'
  nunit.assemblies = ['src\\Coypu.Tests\\Coypu.Tests.csproj']
end

desc 'testdrivers'
nunit :testdrivers => :compile do |nunit|
  nunit.command = 'lib\nspec\NSpecRunnerSTA.exe'
  nunit.assemblies = ["src\\Coypu.Drivers.Tests\\bin\\#{BUILD_CONFIGURATION.downcase}\\Coypu.Drivers.Tests.dll"]
end

desc 'package'
task :package do
  FileUtils.rm_rf('temp')
  FileUtils.mkdir_p('temp')
  [:net35, :net40].each do |version|
    Rake::Task["compile_#{version}"].invoke
    FileUtils.mv('src/Coypu/bin/Release', "temp/#{version}")
  end
  sh 'nuget Pack Coypu.nuspec'
end

desc 'publish'
task :publish => :package do
  package_file = Dir.glob('Coypu*.nupkg').first
  sh "nuget Push #{package_file}"
  FileUtils.rm(package_file)
end
