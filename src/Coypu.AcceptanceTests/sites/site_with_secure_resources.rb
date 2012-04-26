require 'rubygems'
require 'sinatra'

get "/" do
  "<html><head><title>Sinatra has taken the stage</title></head><body>home</body></html>"
end

get "/resource/:value" do
  params[:value]
end

get "/auto_login/?" do
  response.set_cookie "username", {:value => "bob"}
end

get "/restricted_resource/:value" do
  if request.cookies["username"] == "bob"
    params[:value]
  end
end

