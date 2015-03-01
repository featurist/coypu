require 'os'

task :default => :compile

[
  'restore',
  'compile',
  'testdrivers',
  'testacceptance',
  'testunit',
  'testall',
  'package',
  'publish',
  'publishnunit',
  'publishnunit262',
  'publishwatin'
].each do |target|
  desc target
  task target.to_s do
    system "#{'mono ' if OS.posix?}packages/FAKE.Core/tools/FAKE.exe fakefile.fsx #{target}"
  end
end
