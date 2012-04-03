 
nspecs = Dir.glob('**/Coypu.Drivers.Tests/*When_*.cs') 
nspecs.each do |nspec|
  cs = File.read(nspec)
  match = cs.match(/it\["([^"]*)"\] = \(\) =>/)
  
  if match
    cs.gsub!(match[0],"""
    [Test]
    public void #{match[1].gsub(/ /,'_').capitalize()}()
  """)

    File.open(nspec,'w') do
      |f| f.write(cs) 
    end
  end
end