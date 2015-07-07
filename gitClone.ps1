xcopy .\gitClone.ps1 .\submodules\Toggl.Overtime\.

cd .\submodules\Toggl.Overtime\
git commit -a -m "test auto commit from build ${BUILD_BUILDNUMBER}"
git push master origin

#git clone https://colygondev.visualstudio.com/DefaultCollection/_git/MatchPoint%20Cloud won't work