require 'os'

task :default => :compile

namespace :paket do
  ['install', 'update'].each do |command|
    desc "paket #{command}"
    task command.to_s do
      system "#{'mono ' if OS.posix?}.paket/paket.exe #{command}"
    end
  end
end

[
  'restore',
  'compile',
  'testdrivers',
  'testacceptance',
  'testunit',
  'package',
  'publish',
  'publishnunit',
  'publishnunit262',
  'publishwatin'
].each do |target|
  desc target
  task target.to_s => :'paket:install' do
    system "#{'mono ' if OS.posix?}packages/FAKE.Core/tools/FAKE.exe fakefile.fsx #{target}"
  end
end
