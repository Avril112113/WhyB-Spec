# WhyB Spec
Where we ask why is `Why` equal to `Y`...  
WhyB Spec means Yolol Backend Specification  
**NOTICE:** This spec is very new and such, work in progress, expect potentially breaking changes  

The goal of this spec is to allow the separation of the editor and emulation to allow the use of multiple different emulators with a single editor of choice.  
Using this spec for the backend will help development speeds, by not having to worry about making a editor and are able to use existing tools to change the state and test the backend.  
The end users will then be able to use any editor they want with your backend.  
Another benefit of this spec is that multiple users can work on the same backend instance from different editors given that the backend supports multiple connections.  

# The spec overview
TODO  
See [spec.md](spec.md) for the full specification  

# TODO List
- [ ] Get yolol code going back and forth
- [ ] Gettable chip info like internal variables and current line
- [ ] A way for the frontend to know if the backend was restarted or just loss of connection
- [ ] Decide if the backend must support multiple connections or should just be advised
