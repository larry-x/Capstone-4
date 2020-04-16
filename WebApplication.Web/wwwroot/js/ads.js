let tar = document.getElementById("ad");
let tarin = document.createElement('a');

function loadAd (){
    tarin.setAttribute('href', '#');
    tar.appendChild(tarin);
    let image = document.createElement('img');
    image.src = '/images/adimg.jpg';
    image.setAttribute('id', 'clickable');
    tarin.appendChild(image);
}

loadAd();

document.addEventListener("DOMContentLoaded", () => {
    tarin.addEventListener('click', () => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(posi => {
                alert('Your location is ' + posi.coords.latitude + ', ' + posi.coords.longitude
                    + '. Now redirecting you . . .');
            });
          } 
        else {
            alert("Oh, you blocked it eh? Now redirecting you . . .");
          }
        
        setTimeout( () => {
            window.location = "http://www.nyan.cat";
        }, 3000);
        
    });
});