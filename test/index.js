//TWORZYMY SZTUCZNEGO SUPER ! ADMINA

const jwt = require("jsonwebtoken");

let fakeUser = {
    "email": "WKolaj@gmail.com",
    "name" : "Witold kolaj",
    "permissions" : 7
}

let exec = async () => {

    //USUWAM DLA BEZPIEcdZENSTWA!!
    let signedUser = await jwt.sign(fakeUser,"");

    console.log(signedUser);

}

exec();