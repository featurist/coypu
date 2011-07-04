require 'albacore'
require 'ostruct'

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
  [:net35, :net40].each do |version|
    Rake::Task["compile_#{version}"].invoke
    FileUtils.mkdir_p("temp/#{version}")
	
	exclude_files = [
					'src/Coypu/bin/Release/Microsoft.mshtml.dll',
					'src/Coypu/bin/Release/WatiN.Core.dll',
					'src/Coypu/bin/Release/WatiN.Core.xml',
					'src/Coypu/bin/Release/chromedriver.exe',
					'src/Coypu/bin/Release/Coypu.pdb',
					]
	include_files = Dir.glob('src/Coypu/bin/Release/*').reject{|f| exclude_files.include?(f)}
	include_files.each {|f| FileUtils.cp(f, "temp/#{version}")}
  end
  package_file = Dir.glob('Coypu*.nupkg').each {|f| FileUtils.rm(f)}
  sh 'nuget Pack Coypu.nuspec'
end

desc 'publish'
task :publish => :package do
  package_file = Dir.glob('Coypu*.nupkg').first
  sh "nuget Push #{package_file}"
  FileUtils.rm(package_file)
end

namespace :version do
  namespace :bump do
    desc "bump major version"
    task :major do
      bump_version do |version|
        version.major = version.major + 1
        version.minor = 0
	version.patch = 0
      end
    end
    desc "bump minor version"
    task :minor do
      bump_version do |version|
        version.minor = version.minor + 1
        version.patch = 0
      end
    end
    desc "bump patch version"
    task :patch do
      bump_version do |version|
        version.patch = version.patch + 1
      end
    end
  end
end

def bump_version
  version_regex = /<version>(\d+\.\d+\.\d+)<\/version>/
  nuspec = File.read("Coypu.nuspec")
  version_string = nuspec.match(version_regex).captures.first
  version_parts = version_string.split('.')
  version = OpenStruct.new
  version.major = version_parts[0].to_i
  version.minor = version_parts[1].to_i
  version.patch = version_parts[2].to_i
  yield version
  new_version = "#{version.major}.#{version.minor}.#{version.patch}"
  puts "Bumped #{version_string} to #{new_version}"
  new_version_xml = "<version>#{new_version}</version>"
  nuspec.gsub!(version_regex, new_version_xml)
  File.open('Coypu.nuspec', 'w') do |file|
    file.puts nuspec
  end
  sh "git add Coypu.nuspec"
  #sh "git commit -m \"bump version\""
end
