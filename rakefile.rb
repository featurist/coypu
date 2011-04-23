require 'albacore'

BUILD_CONFIGURATION = ENV['BUILD_CONFIGURATION'] || 'Debug'

desc 'compile'
msbuild :compile do |msbuild|
  msbuild.solution = 'src/Coypu.sln'
  msbuild.use :net35
  msbuild.properties :configuration => BUILD_CONFIGURATION
end

desc 'tests'
nunit :test => :compile do |nunit|
  nunit.command = 'lib\NUnit\nunit-console.exe'
  nunit.assemblies = ['src\\Coypu.sln']
end
