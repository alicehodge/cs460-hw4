document.addEventListener('DOMContentLoaded', initializePage, false);

function initializePage() {
    console.log('Page loaded, initializing...')
    const searchButton = document.getElementById('search-button');
    searchButton.addEventListener('click', searchMovies, false);
    const searchResults = document.getElementById('search-results');
    searchResults.addEventListener('click', function(e) {
        if (e.target.closest('.movie-result')) {
            const newEvent = {
                ...e,
                target: e.target.closest('.movie-result')
            };
            getMovieDetails(newEvent);
            getMovieCast(newEvent);
            $('#detail-modal').modal('show');
        }
    }, false);
    const modalCloseButton = document.getElementById('modal-close-button');
    modalCloseButton.addEventListener('click', function() {
        $('#detail-modal').modal('hide');
    }, false);
}

function truncateString(str) {
    if (str.length <= 140) {
      return str
    }
    return str.slice(0, 140) + '...'
}

async function searchMovies(e) {
    e.preventDefault();
    const searchInput = document.getElementById('search-input');
    const searchResultsDiv = document.getElementById('search-results');
    const errorMessageDiv = document.getElementById('error-message');
    const searchTerm = searchInput.value;

    if (!searchTerm) {
        errorMessageDiv.textContent = `Error - Please enter a search term`;
        $(errorMessageDiv).slideDown();
        return;
    }

    const url = `/api/movie/search?title=${searchTerm}`;
    console.log(url);
    try {
        const response = await fetch(url);
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message);
        }
        const movies = await response.json();
        console.log(movies);
        searchResultsDiv.textContent = '';

        const movieTemplate = document.getElementById('movie-template');
        movies.forEach( movie => {
            console.log(movie);
            const clone = movieTemplate.content.cloneNode(true);
            clone.querySelector('#movie-id').id = movie.id;
            const poster = clone.querySelector('#movie-poster');
            poster.src = movie.posterPath;
            poster.width = 100;
            
            clone.querySelector('#movie-title').textContent = movie.title;
            clone.querySelector('#movie-releasedate').textContent = movie.releaseDate;
            clone.querySelector('#movie-overview').textContent = truncateString(movie.overview);
            searchResultsDiv.appendChild(clone);
            });
            $(errorMessageDiv).hide();
        } catch (error) {
            errorMessageDiv.textContent = `Error - ${error.message}`;
            $(errorMessageDiv).slideDown();
        }
}


async function getMovieDetails(e)
{
    const movieId = e.target.id;
    const url = `/api/movie/${movieId}`;
    console.log(url);
    try {
        const response = await fetch(url);
        const movieDetail = await response.json();
        console.log(movieDetail);
        const movieDetailDiv = document.getElementById('movie-detail');
        movieDetailDiv.textContent = '';
        const movieDetailTemplate = document.getElementById('movie-detail-template');
        const clone = movieDetailTemplate.content.cloneNode(true);
        clone.querySelector('#movie-detail-title').textContent = movieDetail.title + (!isNaN(parseInt(movieDetail.releaseDate.slice(-4))) ? ` (${movieDetail.releaseDate.slice(-4)})` : '');
        clone.querySelector('#movie-detail-overview').textContent = movieDetail.overview;
        clone.querySelector('#movie-detail-releasedate').textContent = movieDetail.releaseDate;
        clone.querySelector('#movie-detail-runtime').textContent = movieDetail.runtime;
        clone.querySelector('#movie-detail-revenue').textContent = movieDetail.revenue > 0 ? '$' + Intl.NumberFormat('en-US').format(movieDetail.revenue) : "Unknown Revenue";
        clone.querySelector('#movie-detail-genres').textContent = movieDetail.genres.join(', ');
        clone.querySelector('#movie-detail-backdrop').src = movieDetail.backdropPath
        clone.querySelector('#movie-detail-popularity').textContent = movieDetail.popularity;
        
        movieDetailDiv.appendChild(clone);
    } catch (error) {
        console.log(error);
    }
}

async function getMovieCast(e)
{
    const movieId = e.target.id;
    const url = `/api/movie/${movieId}/cast`;
    console.log(url);
    try {
        const response = await fetch(url);
        const { cast } = await response.json();
        console.log(cast);
        const movieCastDiv = document.getElementById('movie-cast');
        movieCastDiv.textContent = '';
        const movieCastTemplate = document.getElementById('movie-cast-template');
        cast.forEach( castMember => {
            console.log(castMember);
            const clone = movieCastTemplate.content.cloneNode(true);
            clone.querySelector('#movie-cast-name').textContent = castMember.name + ' as';
            clone.querySelector('#movie-cast-character').textContent = castMember.character;
            movieCastDiv.appendChild(clone);
        });
    } catch (error) {
        console.log(error);
    }
}
